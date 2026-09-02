using Common.Infrastructure.Observation;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Combat.UI
{
    public sealed class TotalStatRow : MonoBehaviour
    {
        [SerializeField, Required]
        private Image icon;

        [SerializeField, Required]
        private TMP_Text value, delta;

        private readonly Bindings _bindings = new();

        public void SetTotal(Sprite iconSprite, IReadOnlyObservable<float> total)
        {
            _bindings.Unbind();

            icon.sprite = iconSprite;
            _bindings.Track(total.Observe(OnTotalChanged));

            SetDelta(0f);
        }

        public void SetDelta(float lost)
        {
            var hasLoss = lost > 0f;
            delta.gameObject.SetActive(hasLoss);

            if (hasLoss)
            {
                delta.text = $"(-{lost:0.##})";
            }
        }

        private void OnTotalChanged(float total)
        {
            value.text = $"{total:0.##}";
        }

        private void OnDestroy()
        {
            _bindings.Unbind();
        }
    }
}
