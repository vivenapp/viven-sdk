using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Core.Lua
{
    public static class LuaTypeResolver
    {
        private static readonly Dictionary<string, Type> TypeAliasMap = new(StringComparer.Ordinal)
        {
            { "string", typeof(string) },
            { "int", typeof(int) },
            { "float", typeof(float) },
            { "bool", typeof(bool) },
            { "Vector2", typeof(Vector2) },
            { "Vector3", typeof(Vector3) },
            { "Color", typeof(Color) },
            { "GameObject", typeof(GameObject) },
            { "Transform", typeof(Transform) },
            { "Object", typeof(UnityEngine.Object) },
            { "Component", typeof(Component) },
            { "MonoBehaviour", typeof(MonoBehaviour) },
            { "ScriptableObject", typeof(ScriptableObject) },
            { "VivenScript", typeof(VivenScript) }
        };

        private static readonly Dictionary<string, Type> SerializedTypeCache = new(StringComparer.Ordinal);
        private static readonly Dictionary<string, Type> RawTypeNameCache = new(StringComparer.Ordinal);

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void RegisterCacheInvalidation()
        {
            UnityEditor.AssemblyReloadEvents.afterAssemblyReload += ClearCaches;
        }
#endif

        /// <summary>
        /// 도메인 리로드 후 캐시를 초기화하여 stale Type 참조를 방지합니다.
        /// </summary>
        public static void ClearCaches()
        {
            SerializedTypeCache.Clear();
            RawTypeNameCache.Clear();
        }

        public static Type ResolveSerializedType(string serializedTypeName)
        {
            if (string.IsNullOrEmpty(serializedTypeName))
            {
                return null;
            }

            if (SerializedTypeCache.TryGetValue(serializedTypeName, out var cachedType))
            {
                return cachedType;
            }

            var resolved = Type.GetType(serializedTypeName, throwOnError: false);
            if (resolved != null)
            {
                SerializedTypeCache[serializedTypeName] = resolved;
                return resolved;
            }

            var fullTypeName = serializedTypeName.Split(',')[0].Trim();
            if (string.IsNullOrEmpty(fullTypeName))
            {
                return null;
            }

            var resolvedByFullName = ResolveByFullName(fullTypeName);
            SerializedTypeCache[serializedTypeName] = resolvedByFullName;
            return resolvedByFullName;
        }

        public static Type ResolveTypeFromName(string typeName)
        {
            return ResolveTypeFromName(typeName, out _);
        }

        public static Type ResolveTypeFromName(string typeName, out bool isAmbiguous)
        {
            isAmbiguous = false;

            if (string.IsNullOrWhiteSpace(typeName))
            {
                return null;
            }

            var trimmedTypeName = typeName.Trim();
            if (RawTypeNameCache.TryGetValue(trimmedTypeName, out var cachedType))
            {
                return cachedType;
            }

            if (TypeAliasMap.TryGetValue(trimmedTypeName, out var aliasedType))
            {
                RawTypeNameCache[trimmedTypeName] = aliasedType;
                return aliasedType;
            }

            var directType = Type.GetType(trimmedTypeName, throwOnError: false);
            if (directType != null)
            {
                RawTypeNameCache[trimmedTypeName] = directType;
                return directType;
            }

            var fullNameMatch = ResolveByFullName(trimmedTypeName);
            if (fullNameMatch != null)
            {
                RawTypeNameCache[trimmedTypeName] = fullNameMatch;
                return fullNameMatch;
            }

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type fallbackMatch = null;

            for (var i = 0; i < allAssemblies.Length; i++)
            {
                var assembly = allAssemblies[i];
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types;
                }

                for (var j = 0; j < types.Length; j++)
                {
                    var candidate = types[j];
                    if (candidate == null || candidate.Name != trimmedTypeName)
                    {
                        continue;
                    }

                    if (fallbackMatch != null && fallbackMatch != candidate)
                    {
                        isAmbiguous = true;
                        RawTypeNameCache[trimmedTypeName] = null;
                        return null;
                    }

                    fallbackMatch = candidate;
                }
            }

            RawTypeNameCache[trimmedTypeName] = fallbackMatch;
            return fallbackMatch;
        }

        private static Type ResolveByFullName(string fullTypeName)
        {
            if (string.IsNullOrEmpty(fullTypeName))
            {
                return null;
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (var i = 0; i < assemblies.Length; i++)
            {
                var assemblyType = assemblies[i].GetType(fullTypeName, throwOnError: false);
                if (assemblyType != null)
                {
                    return assemblyType;
                }
            }

            return null;
        }
    }
}
