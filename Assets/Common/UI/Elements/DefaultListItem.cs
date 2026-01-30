using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Elements
{
    public sealed class DefaultListItem : MonoBehaviour
    {
        [field: SerializeField, Required]
        public Image Icon { get; private set; }

        [field: SerializeField, Required]
        public TMP_Text Text { get; private set; }

        public void SetIcon(Sprite sprite)
        {
            Icon.sprite = sprite;
        }

        public void SetText(string newText)
        {
            Text.text = newText;
        }
    }
}