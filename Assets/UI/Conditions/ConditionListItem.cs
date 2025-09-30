using System.Collections.Generic;
using System.Linq;
using Data;
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

        private readonly SortedDictionary<float, Sprite> _thresholdIcons = new();

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
            _progress.CurrentValuePercent.Observe(OnProgressPercentChanged);
        }

        public void AddThreshold(float threshold, Sprite icon)
        {
            _thresholdIcons.Add(threshold, icon);
        }

        private void OnDestroy()
        {
            if (_progress is null) return;

            _progress.CurrentValueText.StopObserving(OnProgressTextChanged);
            _progress.CurrentValuePercent.StopObserving(OnProgressPercentChanged);
        }

        private void OnProgressTextChanged(string text)
        {
            progressText.text = text;
        }

        private void OnProgressPercentChanged(float progressInPercent)
        {
            Sprite selectedIcon;

            if (progressInPercent.IsApproximately(1f))
            {
                selectedIcon = completeIcon;
            }
            else
            {
                Sprite thresholdIcon = null;

                foreach (var (threshold, icon) in _thresholdIcons)
                {
                    if (progressInPercent >= threshold)
                    {
                        thresholdIcon = icon;
                    }
                    else
                    {
                        break;
                    }
                }

                selectedIcon = thresholdIcon ?? incompleteIcon;
            }

            completionImage.sprite = selectedIcon;
        }
    }
}