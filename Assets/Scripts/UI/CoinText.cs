using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class CoinText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            GameManager.Instance.OnCoinValueChanged += OnCoinChanged;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnCoinValueChanged += OnCoinChanged;
        }

        private void OnCoinChanged(uint value)
        {
            _text.text = value.ToString();
        }
    }
}