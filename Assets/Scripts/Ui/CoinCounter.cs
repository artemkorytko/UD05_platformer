using TMPro;
using UnityEngine;

namespace DefaultNamespace.Ui
{
    public class CoinCounter : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            GameManager.Instance.OnCoinValueChanged += OnCoinValue;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnCoinValueChanged -= OnCoinValue;
        }

        private void OnCoinValue(uint value)
        {
            _text.text = $"x{value.ToString()}";
        }
    }
}