using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public abstract class BasePanel : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(ClickButton);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ClickButton);
        }

        protected abstract void ClickButton();
    }
}