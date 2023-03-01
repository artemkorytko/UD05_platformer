using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class UiLevelText : MonoBehaviour
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

        private void OnCoinValue(uint index) // получает из LevelManager, тот в свою очередь из GM
        {
            _text.text = $"LEVEL {index.ToString()}" ;
        }
    }
}