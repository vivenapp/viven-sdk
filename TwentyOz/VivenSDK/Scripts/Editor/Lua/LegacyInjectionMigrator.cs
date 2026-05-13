using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TwentyOz.VivenSDK.Scripts.Core.Lua;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TwentyOz.VivenSDK.Scripts.Editor.Lua
{
    /// <summary>
    /// Legacy Injection 데이터를 바탕으로 VivenScript(.lua) 파일에
    /// ---@type + checkInject 코드를 자동 생성하여 InjectedField 시스템으로 마이그레이션합니다.
    /// </summary>
    public static class LegacyInjectionMigrator
    {
        // Type → Lua 타입 어노테이션 이름 매핑
        private static readonly Dictionary<Type, string> TypeToLuaName = new()
        {
            { typeof(Object), "Object" },
            { typeof(GameObject), "GameObject" },
            { typeof(Vector3), "Vector3" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(bool), "bool" },
            { typeof(string), "string" },
            { typeof(Color), "Color" },
            { typeof(VivenScript), "VivenScript" }
        };

        private static readonly Regex ExistingInjectionPattern = new Regex(
            @"---@type\s+[\.\w]+(?:\s+--.*)?[\r\n]+\s*\w+\s*=\s*(?:checkInject|NullableInject)\(",
            RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// VivenLuaBehaviour의 Legacy Injection 데이터로부터 마이그레이션 코드를 생성하여
        /// 연결된 .lua 파일에 삽입합니다.
        /// </summary>
        /// <returns>마이그레이션된 필드 수. 0이면 마이그레이션할 항목이 없음.</returns>
        public static int Migrate(VivenLuaBehaviour behaviour)
        {
            if (behaviour == null || behaviour.luaScript == null || behaviour.injection == null)
            {
                Debug.LogWarning("[LegacyInjectionMigrator] behaviour, luaScript, 또는 injection이 null입니다.");
                return 0;
            }

            var assetPath = AssetDatabase.GetAssetPath(behaviour.luaScript);
            if (string.IsNullOrEmpty(assetPath))
            {
                Debug.LogWarning("[LegacyInjectionMigrator] luaScript의 에셋 경로를 찾을 수 없습니다.");
                return 0;
            }

            // Legacy 필드 수집
            var fields = CollectLegacyFields(behaviour.injection);
            if (fields.Count == 0)
            {
                Debug.Log("[LegacyInjectionMigrator] 마이그레이션할 Legacy Injection 필드가 없습니다.");
                return 0;
            }

            // 기존 스크립트 내용 읽기
            var scriptContent = File.ReadAllText(assetPath);

            // 이미 존재하는 checkInject/NullableInject 변수명 수집
            var existingNames = CollectExistingInjectionNames(scriptContent);

            // 새로 추가할 필드만 필터링
            var newFields = new List<LegacyField>();
            foreach (var field in fields)
            {
                if (existingNames.Contains(field.Name))
                {
                    Debug.Log($"[LegacyInjectionMigrator] '{field.Name}'은 이미 스크립트에 존재합니다. 건너뜁니다.");
                    continue;
                }
                newFields.Add(field);
            }

            if (newFields.Count == 0)
            {
                Debug.Log("[LegacyInjectionMigrator] 모든 Legacy 필드가 이미 스크립트에 존재합니다.");
                return 0;
            }

            // injection 코드 생성
            var injectionCode = GenerateInjectionCode(newFields);

            // 스크립트 파일에 삽입
            var updatedContent = InsertInjectionCode(scriptContent, injectionCode);
            File.WriteAllText(assetPath, updatedContent);

            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

            Debug.Log($"[LegacyInjectionMigrator] {newFields.Count}개의 Legacy 필드를 '{assetPath}'에 마이그레이션했습니다.");
            return newFields.Count;
        }

        /// <summary>
        /// 마이그레이션 시 생성될 코드를 미리보기용으로 반환합니다.
        /// </summary>
        public static string Preview(VivenLuaBehaviour behaviour)
        {
            if (behaviour?.injection == null) return string.Empty;

            var fields = CollectLegacyFields(behaviour.injection);
            if (fields.Count == 0) return string.Empty;

            string scriptContent = null;
            if (behaviour.luaScript != null)
            {
                var assetPath = AssetDatabase.GetAssetPath(behaviour.luaScript);
                if (!string.IsNullOrEmpty(assetPath) && File.Exists(assetPath))
                {
                    scriptContent = File.ReadAllText(assetPath);
                }
            }

            var existingNames = scriptContent != null
                ? CollectExistingInjectionNames(scriptContent)
                : new HashSet<string>();

            var newFields = new List<LegacyField>();
            foreach (var field in fields)
            {
                if (!existingNames.Contains(field.Name))
                    newFields.Add(field);
            }

            return newFields.Count > 0 ? GenerateInjectionCode(newFields) : string.Empty;
        }

        private struct LegacyField
        {
            public string TypeName;
            public string Name;
        }

        private static List<LegacyField> CollectLegacyFields(Injection injection)
        {
            var fields = new List<LegacyField>();

            CollectFields(fields, "Object", injection.objectValues, v => v?.name);
            CollectFields(fields, "GameObject", injection.gameObjectValues, v => v?.name);
            CollectFields(fields, "Vector3", injection.vector3Values, v => v?.name);
            CollectFields(fields, "float", injection.floatValue, v => v?.name);
            CollectFields(fields, "int", injection.intValue, v => v?.name);
            CollectFields(fields, "bool", injection.boolValue, v => v?.name);
            CollectFields(fields, "string", injection.stringValue, v => v?.name);
            CollectFields(fields, "Color", injection.colorValue, v => v?.name);
            CollectFields(fields, "VivenScript", injection.vivenScriptValue, v => v?.name);

            return fields;
        }

        private static void CollectFields<T>(List<LegacyField> fields, string typeName, T[] values, Func<T, string> getName)
        {
            if (values == null) return;
            for (var i = 0; i < values.Length; i++)
            {
                var name = getName(values[i]);
                if (string.IsNullOrEmpty(name)) continue;
                fields.Add(new LegacyField { TypeName = typeName, Name = name });
            }
        }

        private static HashSet<string> CollectExistingInjectionNames(string scriptContent)
        {
            var names = new HashSet<string>();
            var pattern = new Regex(
                @"(\w+)\s*=\s*(?:checkInject|NullableInject)\(\1\)",
                RegexOptions.Multiline);

            foreach (Match match in pattern.Matches(scriptContent))
            {
                if (match.Groups.Count >= 2)
                {
                    names.Add(match.Groups[1].Value);
                }
            }

            return names;
        }

        private static string GenerateInjectionCode(List<LegacyField> fields)
        {
            var sb = new StringBuilder();

            foreach (var field in fields)
            {
                sb.AppendLine($"---@type {field.TypeName}");
                sb.AppendLine($"{field.Name} = checkInject({field.Name})");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string InsertInjectionCode(string scriptContent, string injectionCode)
        {
            // --region Injection list 또는 --#region Injection list가 있으면 그 안에 추가
            var regionPattern = new Regex(
                @"(--#?region\s+Injection\s+list[^\n]*\n)(.*?)(--#?endregion)",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var match = regionPattern.Match(scriptContent);
            if (match.Success)
            {
                // endregion 앞에 삽입
                var insertPos = match.Groups[3].Index;
                return scriptContent.Insert(insertPos, "\n" + injectionCode);
            }

            // region이 없으면 파일 맨 앞에 삽입
            return injectionCode + scriptContent;
        }
    }
}
