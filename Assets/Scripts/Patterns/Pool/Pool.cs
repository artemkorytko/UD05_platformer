using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class Pool<T> where T : MonoBehaviour
    {
        public Pool( T prefab, int count )
        {
            _prefab = prefab;
            _count = count;
            _сontainer = new GameObject(NAME);
            GeneratePool(_prefab.gameObject, _count);
        }

        private const string NAME = "Pool";
        private int _count;
        private GameObject _сontainer;
        private T _prefab;
        
        
        private List<GameObject> _pool = new List<GameObject>(10);

        private void GeneratePool(GameObject prefab, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var item = GameObject.Instantiate(prefab, _сontainer.transform);
                item.SetActive(false);
                _pool.Add(item);
            }
        }

        public bool TryGetObject(out GameObject item)
        {
            if(_pool.TrueForAll(item => item.activeSelf)) // тип для расширения пула.. как и лист *2
                GeneratePool(_prefab.gameObject, _count);
            
            item = _pool.FirstOrDefault(o => o.activeSelf == false);
            return item != null;
        }

        public void SetObjectInPosition(GameObject item, Vector3 point)
        {
            item.SetActive(true);
            item.transform.position = point;
        }
    }
}