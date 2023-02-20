using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Pool : MonoBehaviour
    {
        public static Pool Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public List<GameObject> GenerateObjects(GameObject prefab, int count)
        {
            List<GameObject> objects = new List<GameObject>(10);
            for (int i = 0; i < count; i++)
            {
                var item = Instantiate(prefab, transform);
                item.SetActive(false);
                objects.Add(item);
            }

            return objects;
        }
    }
}