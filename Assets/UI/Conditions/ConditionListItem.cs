using JetBrains.Annotations;
using Levels.Conditions;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Conditions
{
    public sealed class ConditionListItem : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text descriptionText;

        [SerializeField, Required]
        private TMP_Text progressText;

        [SerializeField, Required]
        private Image completionImage;

        [SerializeField, Required]
        private Image iconImage;

        [SerializeField, Required]
        private Sprite incompleteIcon;

        [SerializeField, Required]
        private Sprite completeIcon;

        private Progress _progress;

        public void Setup(string description, Sprite icon, [CanBeNull] Progress progress = null)
        {
            descriptionText.text = description;
            iconImage.sprite = icon;

            if (progress is null)
            {
                progressText.gameObject.SetActive(false);
                completionImage.sprite = incompleteIcon;
                return;
            }

            _progress = progress;
            _progress.CurrentValueText.Observe(OnProgressTextChanged);
            _progress.IsCompleted.Observe(OnIsCompletedChanged);
        }

        private void OnDestroy()
        {
            if (_progress is null) return;

            _progress.CurrentValueText.StopObserving(OnProgressTextChanged);
            _progress.IsCompleted.Observe(OnIsCompletedChanged);
        }

        private void OnProgressTextChanged(string text)
        {
            progressText.text = text;
        }

        private void OnIsCompletedChanged(bool isComplete)
        {
            completionImage.sprite = isComplete ? completeIcon : incompleteIcon;
        }
    }
}