using System;
using UnityEngine;

public enum Panel
{
    None,
    Menu,
    Game,
    Win,
    Fail,
}

namespace DefaultNamespace.UI
{
    public class UiController : MonoBehaviour
    {
        public static UiController Instance { get; private set; }

        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject failPanel;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetPanel(Panel panel)
        {
            menuPanel.SetActive(panel == Panel.Menu);
            gamePanel.SetActive(panel ==Panel.Game);
            winPanel.SetActive(panel == Panel.Win);
            failPanel.SetActive(panel == Panel.Fail);
        }
    }
    
    
}