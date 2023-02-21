using System;
using DefaultNamespace.UI.Buttons;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class FailPanel : BasePanel
    {
        private Button _resurrectionButton;
        

        private void Awake()
        {
            base.Awake();
            _resurrectionButton = GetComponentInChildren<ResurrectionButton>().GetComponent<Button>();
            _resurrectionButton.onClick.AddListener(OnClickResurrectionButton);
        }

        private void OnEnable()
        {
            _resurrectionButton.gameObject.SetActive(GameManager.Instance.CountCoins >= 10);
        }
        
        private void OnDestroy()
        {
            base.OnDestroy();
            _resurrectionButton.onClick.RemoveListener(OnClickResurrectionButton);
        }

        private void OnClickResurrectionButton()
        {
            GameManager.Instance.OnResurrectionPlayer();
        }

        protected override void OnClickRestarButton()
        {
            GameManager.Instance.StartGame();
        }
    }
}