using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public abstract class RuntimeListSet<T> : ScriptableObject
    {
        private List<T> items = new List<T>();

        public void Initialize()
        {
            items.Clear();
        }

        public List<T> GetList() { return items; }

        public T GetItemIndex(int index)
        {
            return items[0];
        }

        public void AddToList(T item)
        {
            if(!items.Contains(item)) { items.Add(item); }
        }

        public void RemoveFromList(T item)
        {
            if (items.Contains(item)) { items.Remove(item); }
        }

        public int Count() { return items.Count; }
    }
}

