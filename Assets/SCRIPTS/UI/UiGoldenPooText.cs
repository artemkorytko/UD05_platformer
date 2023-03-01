using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class UiGoldenPooText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            
        }

        private void Start()
        {
            GameManager.Instance.OnGoldenPooValueChanged += OnCoinValue;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGoldenPooValueChanged -= OnCoinValue;
        }

        private void OnCoinValue(uint value)
        {
            _text.text = value.ToString();
        }
    }
}