#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using TwentyOz.VivenSDK.Scripts.Core.Lua.InjectField;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Core.Lua
{
    /// <summary>
    /// Viven Lua Scriptable Object
    /// </summary>
    public class VivenScript : ScriptableObject
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/VivenScriptable")]
        public static void CreateAsset()
        {
            // create .lua file in Current Active Folder
            ProjectWindowUtil.CreateAssetWithContent("NewLuaFile.lua", "");
        }
#endif

        /// <summary>
        /// Lua Script String
        /// </summary>
        public string scriptString;

        public Injection InjectionList;

        public ParsedInjectionInfo[] parsedInjectionInfos;
    }

    /// <summary>
    /// Type을 AssemblyQualifiedName으로 직렬화하고, 역직렬화 시 캐싱하는 공통 기반 클래스입니다.
    /// </summary>
    [Serializable]
    public abstract class InjectionInfoBase
    {
        [SerializeField]
        private string typeAssemblyQualifiedName;

        [NonSerialized] private Type _cachedType;
        [NonSerialized] private bool _typeCacheValid;

        public Type Type
        {
            get
            {
                if (!_typeCacheValid)
                {
                    _cachedType = string.IsNullOrEmpty(typeAssemblyQualifiedName)
                        ? null
                        : LuaTypeResolver.ResolveSerializedType(typeAssemblyQualifiedName);
                    _typeCacheValid = true;
                }
                return _cachedType;
            }
            set
            {
                typeAssemblyQualifiedName = value?.AssemblyQualifiedName;
                _cachedType = value;
                _typeCacheValid = true;
            }
        }

        public Type StorageType => ParsedInjectionInfo.NormalizeStorageType(Type);

        public string name;
    }

    [Serializable]
    public class ParsedInjectionInfo : InjectionInfoBase
    {
        public static Type NormalizeStorageType(Type type)
        {
            if (type == null) return null;

            // Keep explicitly supported buckets as-is.
            if (type == typeof(GameObject) || type == typeof(VivenScript))
            {
                return type;
            }

            // Any Unity object-derived type should be stored via UnityEngine.Object bucket.
            if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                return typeof(UnityEngine.Object);
            }

            return type;
        }
    }

    [Serializable]
    public class UIInjectionInfo : InjectionInfoBase
    {
        /// <summary>
        /// Inspector ObjectField에서 사용할 타입입니다.
        /// 원본 타입(예: Transform)이 있으면 해당 타입으로 필터링하고,
        /// 없으면 저장 타입(예: UnityEngine.Object)으로 폴백합니다.
        /// </summary>
        public Type PickerType => Type ?? StorageType;

        [SerializeField]
        private InjectedFieldBase targetField;

        public InjectedFieldBase TargetField
        {
            get => targetField;
            set => targetField = value;
        }

        public object value => targetField.BoxedValue;
    }
}