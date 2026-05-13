using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TwentyOz.VivenSDK.Scripts.Core.Lua;
using UnityEditor.AssetImporters;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TwentyOz.VivenSDK.Scripts.Editor.Lua
{
    [ScriptedImporter(1, "lua")]
    public class TwozLuaImporter : ScriptedImporter
    {
        private static readonly Regex InjectionPattern = new Regex(
            @"---@type\s+([\.\w]+)(?: --.*)?[\r\n]+\s*(\w+) = (?:checkInject|NullableInject)\(\2\)(?: --.*)?",
            RegexOptions.Multiline | RegexOptions.Compiled);

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var createdAsset = ScriptableObject.CreateInstance<VivenScript>();
            createdAsset.scriptString = System.IO.File.ReadAllText(ctx.assetPath);
            ExtractInjectionList(createdAsset);
            ctx.AddObjectToAsset("VivenScript", createdAsset);
            ctx.SetMainObject(createdAsset);
        }

        public void ExtractInjectionList(VivenScript script)
        {
            if (script == null) return;

            script.InjectionList ??= new Injection();
            InitializeInjectionArrays(script.InjectionList);
            var parsedInjectionList = ParseCheckInjectList(script.scriptString);
            script.parsedInjectionInfos = parsedInjectionList.ToArray();
            if (parsedInjectionList.Count == 0) return;

            var injectionDict = GroupInjectionsByType(parsedInjectionList);
            ProcessAllInjectionTypes(script.InjectionList, injectionDict);
        }

        private void InitializeInjectionArrays(Injection injection)
        {
            injection.stringValue ??= Array.Empty<StringValue>();
            injection.floatValue ??= Array.Empty<FloatValue>();
            injection.intValue ??= Array.Empty<IntValue>();
            injection.boolValue ??= Array.Empty<BoolValue>();
            injection.vector3Values ??= Array.Empty<Vector3Value>();
            injection.colorValue ??= Array.Empty<ColorValue>();
            injection.vivenScriptValue ??= Array.Empty<VivenScriptValue>();
            injection.gameObjectValues ??= Array.Empty<GameObjectValue>();
            injection.objectValues ??= Array.Empty<ObjectValue>();
        }

        private Dictionary<Type, List<ParsedInjectionInfo>> GroupInjectionsByType(List<ParsedInjectionInfo> injectionList)
        {
            var injectionDict = new Dictionary<Type, List<ParsedInjectionInfo>>();

            foreach (var injection in injectionList)
            {
                if (injection?.Type == null) continue;
                var storageType = ParsedInjectionInfo.NormalizeStorageType(injection.Type);
                if (storageType == null) continue;

                if (!injectionDict.TryGetValue(storageType, out var list))
                {
                    list = new List<ParsedInjectionInfo>();
                    injectionDict[storageType] = list;
                }

                list.Add(injection);
            }

            return injectionDict;
        }

        private void ProcessAllInjectionTypes(Injection injection,
            Dictionary<Type, List<ParsedInjectionInfo>> injectionDict)
        {
            foreach (var kvp in injectionDict)
            {
                var storageType = kvp.Key;
                var injections = kvp.Value;

                if (storageType == typeof(string))
                    ProcessInjections(ref injection.stringValue, injections, n => new StringValue { name = n }, v => v.name);
                else if (storageType == typeof(float))
                    ProcessInjections(ref injection.floatValue, injections, n => new FloatValue { name = n }, v => v.name);
                else if (storageType == typeof(int))
                    ProcessInjections(ref injection.intValue, injections, n => new IntValue { name = n }, v => v.name);
                else if (storageType == typeof(bool))
                    ProcessInjections(ref injection.boolValue, injections, n => new BoolValue { name = n }, v => v.name);
                else if (storageType == typeof(Vector3))
                    ProcessInjections(ref injection.vector3Values, injections, n => new Vector3Value { name = n }, v => v.name);
                else if (storageType == typeof(Color))
                    ProcessInjections(ref injection.colorValue, injections, n => new ColorValue { name = n }, v => v.name);
                else if (storageType == typeof(VivenScript))
                    ProcessInjections(ref injection.vivenScriptValue, injections, n => new VivenScriptValue { name = n }, v => v.name);
                else if (storageType == typeof(GameObject))
                    ProcessInjections(ref injection.gameObjectValues, injections, n => new GameObjectValue { name = n }, v => v.name);
                else if (storageType == typeof(Object))
                    ProcessInjections(ref injection.objectValues, injections, n => new ObjectValue { name = n }, v => v.name);
                else
                    Debug.LogWarning($"Unsupported Injection(Legacy) type: {storageType.Name}, Skip inject");
            }
        }

        private static void ProcessInjections<T>(ref T[] existing, List<ParsedInjectionInfo> injections,
            Func<string, T> create, Func<T, string> getName)
        {
            var list = existing?.ToList() ?? new List<T>();
            var existingNames = new HashSet<string>(list.Count);
            foreach (var item in list)
            {
                var name = getName(item);
                if (name != null) existingNames.Add(name);
            }

            foreach (var injection in injections)
            {
                if (existingNames.Contains(injection.name)) continue;
                list.Add(create(injection.name));
                existingNames.Add(injection.name);
            }

            existing = list.ToArray();
        }

        public List<ParsedInjectionInfo> ParseCheckInjectList(string scriptString)
        {
            if (string.IsNullOrEmpty(scriptString))
            {
                Debug.LogWarning("Script string is null or empty");
                return new List<ParsedInjectionInfo>();
            }

            var matches = InjectionPattern.Matches(scriptString);
            var injectionInfos = new List<ParsedInjectionInfo>();

            foreach (Match match in matches)
            {
                if (match.Groups.Count < 2) continue;

                string typeName = match.Groups[1].Value;
                string fieldName = match.Groups[2].Value;

                if (typeName == "TYPE_OF_VARIABLE") continue;

                var resolvedType = LuaTypeResolver.ResolveTypeFromName(typeName, out var isAmbiguous);

                if (resolvedType == null)
                {
                    if (isAmbiguous)
                    {
                        Debug.LogWarning(
                            $"Ambiguous type name '{typeName}' found in multiple assemblies. Use full type name.");
                    }
                    Debug.LogWarning($"Could not resolve injection type: {typeName}.");
                }

                injectionInfos.Add(new ParsedInjectionInfo
                {
                    Type = resolvedType,
                    name = fieldName
                });
            }

            return injectionInfos;
        }
    }
}