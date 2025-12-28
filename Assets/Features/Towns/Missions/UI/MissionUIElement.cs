using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Missions.UI
{
    public sealed class MissionUIElement : MonoBehaviour
    {
        [SerializeField]
        private Image icon;

        [SerializeField]
        private TMP_Text countText;

        private int _totalAmount;

        public void Setup(Sprite goodIcon, int currentAmount, int totalAmount)
        {
            _totalAmount = totalAmount;
            icon.sprite = goodIcon;
            countText.text = currentAmount.ToString();

            UpdateCurrentAmount(currentAmount);
        }

        public void UpdateCurrentAmount(int currentAmount)
        {
            countText.text = $"{currentAmount}/{_totalAmount}";
        }
    }
}