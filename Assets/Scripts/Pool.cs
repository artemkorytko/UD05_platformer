﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class Pool : MonoBehaviour
    { 
        [SerializeField] protected Transform сontainer;
        
        private List<GameObject> _pool = new List<GameObject>();

        protected void GeneratePool(GameObject prefab, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var item = Instantiate(prefab, сontainer);
                item.SetActive(false);
                _pool.Add(item);
            }
        }

        protected bool TryGetItem(out GameObject item)
        {
            item = _pool.FirstOrDefault(i => i.activeSelf == false);
            return item != null;
        }

        protected void SetItem(GameObject item, Vector3 point)
        {
            item.SetActive(true);
            item.transform.position = point;
        }
    }
}