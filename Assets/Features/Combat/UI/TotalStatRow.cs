using Common.Infrastructure.Observation;
using Common.UI.Tooltips;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Combat.UI
{
    public sealed class TotalStatRow : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text value, delta;

        [SerializeField, Required]
        private SimpleTooltipHandler tooltip;

        private readonly Bindings _bindings = new();

        public void SetTotal(IReadOnlyObservable<float> total, string title)
        {
            _bindings.Unbind();

            _bindings.Track(total.Observe(OnTotalChanged));
            tooltip.SetData(title);
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