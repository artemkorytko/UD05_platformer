using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class UiGemText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            GameManager.Instance.OnGemValueChanged += OnCoinValue;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGemValueChanged -= OnCoinValue;
        }

        private void OnCoinValue(uint value)
        {
            _text.text = value.ToString();
        }
    }
}