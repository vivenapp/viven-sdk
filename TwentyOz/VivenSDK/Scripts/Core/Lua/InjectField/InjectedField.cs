using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Core.Lua.InjectField
{
    [Serializable]
    public abstract class InjectedFieldBase
    {
        [SerializeField]
        public string name;

        public abstract object BoxedValue { get; set; }
    }

    [Serializable]
    public class InjectedField<T> : InjectedFieldBase
    {
        [SerializeField] public T value;

        public override object BoxedValue
        {
            // getter: 필드 value를 object로 반환
            get => value;
            // setter: 프로퍼티 매개변수 value를 T로 캐스팅하여 필드 this.value에 대입
            set => this.value = (T)value;
        }
    }

    public interface IInjectedFieldListAdapter
    {
        Type ValueType { get; }
        IList RawList { get; }
        bool TryUpsert(string name, object value);
        bool TryGetValue(string name, out object value);
        bool TryGetField(string name, out InjectedFieldBase field);
        void Clear();
        void Print();
    }

    public interface IInjectedFieldListAdapter<T> : IInjectedFieldListAdapter
    {
        bool TryUpsert(string name, T value);
        bool TryGetValue(string name, out T value);
    }

    public sealed class InjectedFieldListAdapter<T> : IInjectedFieldListAdapter<T>
    {
        private readonly List<InjectedField<T>> _list;

        public InjectedFieldListAdapter(List<InjectedField<T>> list)
        {
            _list = list;
        }

        public Type ValueType => typeof(T);
        public IList RawList => _list;

        public bool TryUpsert(string name, object value)
        {
            if (!TryCastValue(value, out T typedValue)) return false;
            return TryUpsert(name, typedValue);
        }

        public bool TryUpsert(string name, T value)
        {
            var idx = _list.FindIndex(e => e.name == name);
            if (idx == -1)
            {
                _list.Add(new InjectedField<T> { name = name, value = value });
            }
            else
            {
                _list[idx].value = value;
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
            for (var i = 0; i < _list.Count; i++)
            {
                if (_list[i].name != name) continue;
                value = _list[i].value;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGetField(string name, out InjectedFieldBase field)
        {
            for (var i = 0; i < _list.Count; i++)
            {
                if (_list[i].name != name) continue;
                field = _list[i];
                return true;
            }

            field = null;
            return false;
        }

        public void Clear()
        {
            _list.Clear();
        }

        public void Print()
        {
            _list.ForEach(p => Debug.Log(p.name + ": " + p.value));
        }

        private static bool TryCastValue(object value, out T typedValue)
        {
            if (value is T casted)
            {
                typedValue = casted;
                return true;
            }

            if (value == null)
            {
                typedValue = default;
                return true;
            }

            typedValue = default;
            return false;
        }
    }
}