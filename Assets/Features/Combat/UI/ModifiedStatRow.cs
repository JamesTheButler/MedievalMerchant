using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.UI.Tooltips;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Combat.UI
{
    public sealed class ModifiedStatRow : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text baseValue, modifier, result;

        [SerializeField, Required]
        private GameObject modifierGroup;

        [SerializeField, Required]
        private ModifiableTooltipHandler tooltip;

        private readonly Bindings _bindings = new();
        private ModifiableVariable _stat;

        public void SetStat(ModifiableVariable stat)
        {
            Unsubscribe();

            _stat = stat;

            if (_stat == null)
                return;


            tooltip.SetData(stat);

            _bindings.Track(_stat.Observe(OnValueChanged));
            _stat.ModifiersChanged += Refresh;

            Refresh();
        }

        private void OnValueChanged(float _)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (_stat == null)
                return;

            baseValue.text = $"{_stat.BaseValue:0.##}";
            modifierGroup.SetActive(_stat.IsModified);

            if (!_stat.IsModified)
                return;

            modifier.text = $"{_stat.TotalPercentage * 100:+0.#;-0.#;0.#}% =";
            result.text = $"{_stat.Value:0.##}";
        }

        private void Unsubscribe()
        {
            _bindings.Unbind();

            if (_stat == null)
                return;

            _stat.ModifiersChanged -= Refresh;
            _stat = null;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
    }
}