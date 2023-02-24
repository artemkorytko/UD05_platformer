using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Patterns.Singleton;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class GamePanel : BaseSingleton<GamePanel>
    {
        [SerializeField] private Heart prefabHeart;
        [SerializeField] private HealthBar _healthBar;
        
        private List<GameObject> _listHeards = new List<GameObject>(10);
        
        private void Awake()
        {
            for (int i = 0; i < 10; i++)
            {
              var heard = Instantiate(prefabHeart.gameObject, _healthBar.transform);
              heard.SetActive(false);
              _listHeards.Add(heard);
            }
        }
        
        
        private void OnDisable()
        {
            foreach (var heard in _listHeards)
                heard?.SetActive(false);
        }
        
        public void RemoveHeard()
        {
            var heard =_listHeards.FirstOrDefault(item => item.activeSelf);
            heard?.SetActive(false);
        }

        public void AddHeard(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var heard = _listHeards.FirstOrDefault(item => item.activeSelf == false);
                heard?.SetActive(true);
            }
        }

    }
}