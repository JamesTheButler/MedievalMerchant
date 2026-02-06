using Features.Levels.Conditions.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Levels.Conditions.UI
{
    public sealed class InGameConditionListItem : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text descriptionText, progressText;

        [SerializeField, Required]
        private Image completionImage, iconImage;

        private Progress _progress;

        public void Setup(string description, Sprite icon)
        {
            descriptionText.text = description;
            iconImage.sprite = icon;
        }

        public void SetProgressText(string text)
        {
            progressText.text = text;
        }

        public void SetProgressIcon(Sprite icon)
        {
            completionImage.sprite = icon;
        }
    }
}