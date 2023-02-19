using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class UiCarrotText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            
        }

        private void Start()
        {
            GameManager.Instance.OnCarrotValueChanged += OnCoinValue;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnCarrotValueChanged -= OnCoinValue;
        }

        private void OnCoinValue(uint value)
        {
            _text.text = value.ToString();
        }
    }
}