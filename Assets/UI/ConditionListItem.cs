using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class ConditionListItem : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text descriptionText;

        [SerializeField, Required]
        private TMP_Text progressText;

        [SerializeField, Required]
        private Image iconImage;

        public void Setup(string description, string progress, Sprite icon)
        {
            descriptionText.text = description;
            progressText.text = progress;
            iconImage.sprite = icon;
            progressText.gameObject.SetActive(false);
        }
    }
}