using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public abstract class RuntimeDictSet<K,V> : ScriptableObject
    {
        private Dictionary<K,V> items = new Dictionary<K, V>();

        public void Initialize()
        {
            items.Clear();
        }

        public bool Contains(K key)
        {
            if (items.ContainsKey(key))
            {
                return true;
            }
            return false;
        }

        public V GetValue(K key)
        {
            if(items.ContainsKey(key))
            {
                return items[key];
            }
            return default(V);
        }

        public void AddToSet(K key, V value)
        {
            if(!items.ContainsKey(key))
            {
                items.Add(key, value);
            }
        }

        public void AddToSetOrUpdate(K key, V value)
        {
            if (items.ContainsKey(key))
            {
                items[key] = value;
            }
            else
            {
                AddToSet(key, value);
            }
        }

        public void RemoveFromSet(K key)
        {
            if (items.ContainsKey(key))
            {
                items.Remove(key);
            }
        }

        public int Count()
        {
            return items.Count;
        }
    }
}

