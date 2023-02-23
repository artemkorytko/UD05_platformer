using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class ReviveButtonComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private TextMeshProUGUI _text;

        private void OnEnable()
        {
            if (GameManager.Instance.Data.Coins < GameManager.Instance.ResurrectionCost)
            {
                _text.text = $"Current coins {GameManager.Instance.Data.Coins.ToString()}\nNot enough coins to resurrect!\nGame Over!!!";
                _buttonText.text = $"Resurrection (-{GameManager.Instance.ResurrectionCost})";
            }
            else
            {
                _text.text = $"Current coins {GameManager.Instance.Data.Coins.ToString()}";
                _buttonText.text = $"Resurrection (-{GameManager.Instance.ResurrectionCost})";
            }
        }
    }
}