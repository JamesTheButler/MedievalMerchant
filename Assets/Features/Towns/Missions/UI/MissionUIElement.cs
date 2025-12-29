using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Missions.UI
{
    public sealed class MissionUIElement : MonoBehaviour
    {
        [SerializeField, Required]
        private Image icon;

        [SerializeField, Required]
        private Button abortButton;

        [SerializeField, Required]
        private TMP_Text countText;

        private int _totalAmount;
        private Action _abortCallback;

        private void Awake()
        {
            abortButton.onClick.AddListener(AbortButtonClicked);
        }

        public void Setup(Sprite goodIcon, int currentAmount, int totalAmount, Action abortCallback)
        {
            _abortCallback = abortCallback;
            _totalAmount = totalAmount;
            icon.sprite = goodIcon;
            countText.text = currentAmount.ToString();

            UpdateCurrentAmount(currentAmount);
        }

        public void UpdateCurrentAmount(int currentAmount)
        {
            countText.text = $"{currentAmount}/{_totalAmount}";
        }

        private void AbortButtonClicked()
        {
            _abortCallback?.Invoke();
        }
    }
}