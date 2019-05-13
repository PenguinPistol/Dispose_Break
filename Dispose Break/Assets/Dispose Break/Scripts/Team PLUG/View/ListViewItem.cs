using System;
using UnityEngine;
using UnityEngine.Events;

namespace com.TeamPlug.View
{
    public abstract class ListViewItem<T> : MonoBehaviour
    {
        protected int index;
        protected T data;

        public T Data { get { return data; } }
        public int Index { get { return index; } }

        public virtual void Init(T _data, int _index)
        {
            data = _data;
            index = _index;
        }
    }
}
