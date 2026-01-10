using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Elements
{
    public sealed class TextWithIconElement : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text text;

        [SerializeField, Required]
        private Image icon;

        public void SetUp(string newText, Sprite newIcon)
        {
            text.text = newText;
            icon.sprite = newIcon;
        }
    }
}