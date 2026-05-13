using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Core.Lua.InjectField
{
    public sealed class ValueTypeInjectedFieldListAdapter<T> : IInjectedFieldListAdapter<T>
        where T : struct
    {
        private readonly List<InjectedField<T>> list;

        public ValueTypeInjectedFieldListAdapter(List<InjectedField<T>> list)
        {
            this.list = list;
        }

        public Type ValueType => typeof(T);
        public IList RawList => list;

        public bool TryUpsert(string name, object value)
        {
            if (value == null)
            {
                return TryUpsert(name, default);
            }

            if (value is T typedValue)
            {
                return TryUpsert(name, typedValue);
            }

            return false;
        }

        public bool TryUpsert(string name, T value)
        {
            var idx = list.FindIndex(e => e.name == name);
            if (idx == -1)
            {
                list.Add(new InjectedField<T> { name = name, value = value });
            }
            else
            {
                list[idx].value = value;
            }

            return true;
        }

        public bool TryGetValue(string name, out object value)
        {
            if (TryGetValue(name, out T typedValue))
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGetValue(string name, out T value)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].name != name) continue;
                value = list[i].value;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGetField(string name, out InjectedFieldBase field)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].name != name) continue;
                field = list[i];
                return true;
            }

            field = null;
            return false;
        }

        public void Clear()
        {
            list.Clear();
        }

        public void Print()
        {
            list.ForEach(p => Debug.Log(p.name + ": " + p.value));
        }
    }
}
