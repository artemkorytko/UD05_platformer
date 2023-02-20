using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private Heart prefabHeart;
        
        private List<GameObject> _listHeards = new List<GameObject>(10);

        public static GamePanel Instance { get; private set; }

        private HealthBar _healthBar;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _healthBar = GetComponentInChildren<HealthBar>();
        }

        public void SetCountHealth(int countHealth)
        {
            for (int i = 0; i < countHealth; i++)
            {
              var heard = Instantiate(prefabHeart.gameObject, _healthBar.transform);
              _listHeards.Add(heard);
            }
        }

        public void RemoveHeard()
        {
            var heard =_listHeards.FirstOrDefault(item => item.activeSelf);
            heard.SetActive(false);
        }

        public void AddHeard()
        {
            var heard = _listHeards.FirstOrDefault(item => item.activeSelf == false);
            heard.SetActive(true);
        }

    }
}