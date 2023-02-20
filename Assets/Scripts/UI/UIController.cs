using UnityEngine;

namespace DefaultNamespace.UI
{
    public enum Panel
    {
        None,
        Menu,
        Game,
        Fail,
        Win
    }

    public class UIController : MonoBehaviour
    {
        public static UIController Instance { get; private set; }
        
         private GamePanel _gamePanel;
         private FailPanel _failPanel;
         private WinPanel _winPanel;
         private MenuPanel _menuPanel;

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
            _gamePanel = GetComponentInChildren<GamePanel>(true);
            _failPanel = GetComponentInChildren<FailPanel>(true);
            _winPanel = GetComponentInChildren<WinPanel>(true);
            _menuPanel = GetComponentInChildren<MenuPanel>(true);
        }

        public void SetPanels(Panel panel)
        {
            _menuPanel.gameObject.SetActive(panel == Panel.Menu);
            _gamePanel.gameObject.SetActive(panel == Panel.Game);
            _winPanel.gameObject.SetActive(panel == Panel.Win);
            _failPanel.gameObject.SetActive(panel == Panel.Fail);
        }
    }
}