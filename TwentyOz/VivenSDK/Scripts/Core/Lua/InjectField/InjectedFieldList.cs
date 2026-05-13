using System;
using System.Collections.Generic;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Core.Lua.InjectField
{
    [Serializable]
    public class InjectedFieldList
    {
        public List<InjectedField<UnityEngine.Object>> objectValues = new();
        public List<InjectedField<GameObject>> gameObjectValues = new();
        public List<InjectedField<Vector2>> vector2Values = new();
        public List<InjectedField<Vector3>> vector3Values = new();
        public List<InjectedField<float>> floatValues = new();
        public List<InjectedField<int>> intValues = new();
        public List<InjectedField<bool>> boolValues = new();
        public List<InjectedField<string>> stringValues = new();
        public List<InjectedField<Color>> colorValues = new();
        public List<InjectedField<VivenScript>> vivenScriptValues = new();

        private Dictionary<Type, IInjectedFieldListAdapter> typeAdapters;
        private IInjectedFieldListAdapter objectAdapter;
        private List<IInjectedFieldListAdapter> orderedAdapters;

        public List<InjectedField<T>> GetList<T>()
        {
            return GetList(typeof(T)) as List<InjectedField<T>>;
        }

        public object GetList(Type type)
        {
            return TryGetAdapter(type, out var adapter) ? adapter.RawList : null;
        }

        public bool Upsert<T>(string name, T value)
        {
            if (TryGetTypedAdapter(out IInjectedFieldListAdapter<T> typedAdapter))
            {
                return typedAdapter.TryUpsert(name, value);
            }

            return TryGetAdapter(typeof(T), out var adapter) && adapter.TryUpsert(name, value);
        }

        public bool Upsert(Type type, string name, object value)
        {
            return TryGetAdapter(type, out var adapter) && adapter.TryUpsert(name, value);
        }

        public object GetValue(Type type, string name)
        {
            if (TryGetAdapter(type, out var adapter) && adapter.TryGetValue(name, out var value))
                return value;

            return null;
        }

        public bool TryGetField(Type type, string name, out InjectedFieldBase result)
        {
            result = null;
            return TryGetAdapter(type, out var adapter) && adapter.TryGetField(name, out result);
        }

        public bool IsValidType(Type type)
        {
            return TryGetAdapter(type, out _);
        }

        public void Clear()
        {
            EnsureAdapters();
            for (var i = 0; i < orderedAdapters.Count; i++)
            {
                orderedAdapters[i].Clear();
            }
        }

        public void Print()
        {
            EnsureAdapters();
            for (var i = 0; i < orderedAdapters.Count; i++)
            {
                orderedAdapters[i].Print();
            }
        }

        public void RemoveFieldsNotIn(Dictionary<Type, HashSet<string>> validNamesByType)
        {
            if (validNamesByType == null) return;
            EnsureAdapters();

            for (var i = 0; i < orderedAdapters.Count; i++)
            {
                var adapter = orderedAdapters[i];
                var list = adapter.RawList;
                if (list == null) continue;

                validNamesByType.TryGetValue(adapter.ValueType, out var validNames);

                for (var j = list.Count - 1; j >= 0; j--)
                {
                    if (list[j] is InjectedFieldBase field &&
                        !string.IsNullOrEmpty(field.name) &&
                        validNames != null &&
                        validNames.Contains(field.name))
                    {
                        continue;
                    }

                    list.RemoveAt(j);
                }
            }
        }

        private bool TryGetAdapter(Type type, out IInjectedFieldListAdapter adapter)
        {
            EnsureAdapters();

            if (typeAdapters.TryGetValue(type, out adapter)) return true;

            // 다른 타입으로 추론 불가능한 타입만 ObjectValues에 저장
            if (type != typeof(object) && typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                adapter = objectAdapter;
                return true;
            }

            adapter = null;
            return false;
        }

        private bool TryGetTypedAdapter<T>(out IInjectedFieldListAdapter<T> adapter)
        {
            EnsureAdapters();

            if (typeAdapters.TryGetValue(typeof(T), out var baseAdapter) &&
                baseAdapter is IInjectedFieldListAdapter<T> typedAdapter)
            {
                adapter = typedAdapter;
                return true;
            }

            adapter = null;
            return false;
        }

        private void EnsureAdapters()
        {
            if (typeAdapters != null) return;

            orderedAdapters = new List<IInjectedFieldListAdapter>
            {
                new InjectedFieldListAdapter<UnityEngine.Object>(objectValues),
                new InjectedFieldListAdapter<GameObject>(gameObjectValues),
                new ValueTypeInjectedFieldListAdapter<Vector2>(vector2Values),
                new ValueTypeInjectedFieldListAdapter<Vector3>(vector3Values),
                new ValueTypeInjectedFieldListAdapter<float>(floatValues),
                new ValueTypeInjectedFieldListAdapter<int>(intValues),
                new ValueTypeInjectedFieldListAdapter<bool>(boolValues),
                new InjectedFieldListAdapter<string>(stringValues),
                new ValueTypeInjectedFieldListAdapter<Color>(colorValues),
                new InjectedFieldListAdapter<VivenScript>(vivenScriptValues)
            };

            objectAdapter = orderedAdapters[0];
            typeAdapters = new Dictionary<Type, IInjectedFieldListAdapter>();
            for (var i = 1; i < orderedAdapters.Count; i++)
            {
                typeAdapters.Add(orderedAdapters[i].ValueType, orderedAdapters[i]);
            }
        }
    }
}