using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Goods
{
    public sealed class GoodTooltip : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text nameText, priceText;

        [SerializeField, Required]
        private Image tierImage, regionImage;

        public void SetUp(string goodName, float basePrice, Sprite tierIcon, Sprite regionIcon)
        {
            nameText.text = goodName;
            priceText.text = $"{basePrice:0.##}";
            tierImage.sprite = tierIcon;
            regionImage.sprite = regionIcon;
        }
    }
}