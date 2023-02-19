using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class UiBerryText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            
        }

        private void Start()
        {
            GameManager.Instance.OnBerryValueChanged += OnCoinValue;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnBerryValueChanged -= OnCoinValue;
        }

        private void OnCoinValue(uint value)
        {
            _text.text = value.ToString();
        }
    }
}