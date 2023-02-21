using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public abstract class BasePanel : MonoBehaviour
    {
        private Button _button;

        protected void Awake()
        {
            _button = GetComponentInChildren<Button>();
        }

        protected void Start()
        {
            _button.onClick.AddListener(OnClickRestarButton);
        }

        protected void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClickRestarButton);
        }

        protected abstract void OnClickRestarButton();
    }
}