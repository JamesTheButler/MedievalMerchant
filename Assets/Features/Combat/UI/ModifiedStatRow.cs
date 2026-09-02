using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Combat.UI
{
    public sealed class ModifiedStatRow : MonoBehaviour
    {
        [SerializeField, Required]
        private Image baseIcon, resultIcon;

        [SerializeField, Required]
        private TMP_Text baseValue, modifier, result;

        [SerializeField, Required]
        private GameObject modifierGroup;

        private readonly Bindings _bindings = new();
        private ModifiableVariable _stat;

        public void SetStat(Sprite icon, ModifiableVariable stat)
        {
            Unsubscribe();

            _stat = stat;
            baseIcon.sprite = icon;
            resultIcon.sprite = icon;

            if (_stat == null)
                return;

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

            var abs = Mathf.Abs(_stat.TotalPercentage);
            var sign = _stat.TotalPercentage.Sign();
            modifier.text = $" {sign} {abs * 100:0.##}% =";
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