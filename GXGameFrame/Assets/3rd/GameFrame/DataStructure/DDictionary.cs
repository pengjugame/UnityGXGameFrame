﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace GameFrame
{
    public class DDictionary<T, K, V>:  IEnumerable<KeyValuePair<T, Dictionary<K,V>>>
    {
        private readonly Dictionary<T, Dictionary<K, V>> DDTKV = new();
        public void Add(T t, K k, V v)
        {
            if (!DDTKV.TryGetValue(t, out Dictionary<K, V> kv))
            {
                kv = new();
                DDTKV.Add(t, kv);
            }

            if (kv.TryGetValue(k, out V value))
            {
                Debugger.LogWarning($"kv  have t:{typeof(K)}");
            }
            else
            {
                kv.Add(k, v);
            }
        }

        public void RemoveTkey(T t)
        {
            if (!DDTKV.TryGetValue(t, out Dictionary<K, V> kv))
            {
                return;
            }

            DDTKV.Remove(t);
        }

        public V RemoveKkey(T t, K k)
        {
            if (!DDTKV.TryGetValue(t, out Dictionary<K, V> kv) || !kv.TryGetValue(k, out V value))
            {
                return default(V);
            }

            kv.Remove(k);
            return value;
        }

        public V GetVValue(T t, K k)
        {
            if (!DDTKV.TryGetValue(t, out Dictionary<K, V> kv) || !kv.TryGetValue(k, out V value))
            {
                return default(V);
            }

            return value;
        }
        
        public  Dictionary<K, V> GetValue(T t)
        {
            if (!DDTKV.TryGetValue(t, out Dictionary<K, V> kv))
            {
                return null;
            }

            return kv;
        }

        public bool ContainsTk(T t, K k)
        {
            if (!DDTKV.TryGetValue(t, out Dictionary<K, V> kv) || !kv.ContainsKey(k))
            {
                return false;
            }
            return true;
        }



        IEnumerator<KeyValuePair<T, Dictionary<K,V>>> IEnumerable<KeyValuePair<T, Dictionary<K,V>>>.GetEnumerator()
        {
            return DDTKV.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return DDTKV.GetEnumerator();
        }
    }
}