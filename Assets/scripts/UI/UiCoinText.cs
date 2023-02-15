using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class UiCoinText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            
        }

        private void Start()
        {
            GameManager.Instance.OnCoinValueChanged += OnCoinValue;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnCoinValueChanged -= OnCoinValue;
        }

        private void OnCoinValue(uint value)
        {
            _text.text = value.ToString();
        }
    }
}