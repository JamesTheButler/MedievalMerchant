using Common.Infrastructure.Observation;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Combat.UI
{
    public sealed class UnitCountRow : MonoBehaviour
    {
        [SerializeField, Required]
        private Image icon;

        [SerializeField, Required]
        private TMP_Text count, delta;

        [SerializeField]
        private bool showIcon = true;

        private readonly Bindings _bindings = new();

        private int _maxCount;

        public void SetCount(Sprite iconSprite, IReadOnlyObservable<int> alive, int maxCount)
        {
            _bindings.Unbind();
            _maxCount = maxCount;

            icon.sprite = iconSprite;
            icon.gameObject.SetActive(showIcon && iconSprite != null);

            _bindings.Track(alive.Observe(OnAliveChanged));

            SetDelta(0);
        }

        public void SetDelta(int lost)
        {
            var hasLoss = lost > 0;
            delta.gameObject.SetActive(hasLoss);

            if (hasLoss)
            {
                delta.text = $"(-{lost})";
            }
        }

        private void OnAliveChanged(int alive)
        {
            count.text = $"{alive}/{_maxCount}";
        }

        private void OnDestroy()
        {
            _bindings.Unbind();
        }
    }
}