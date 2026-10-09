using Common.Config;
using Common.Infrastructure;
using Common.UI.Tooltips;
using Features.Combat.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Combat.UI
{
    public sealed class UnitTokenTooltip : TooltipBase<CombatUnit>
    {
        [SerializeField, Required]
        private TMP_Text unitName, health, damageTaken;

        [SerializeField, Required]
        private Image tierIcon;

        private TierResources _tierResources;

        protected override void Awake()
        {
            base.Awake();

            _tierResources = ResourceManager.Instance.TierResources;
        }

        public override void Reset()
        {
            unitName.text = string.Empty;
            health.text = string.Empty;
            damageTaken.text = string.Empty;
        }

        protected override void UpdateUI(CombatUnit unit)
        {
            tierIcon.sprite = _tierResources.GetTierIconByLevel(unit.Combatant.Level);
            health.text = $"{unit.Health.Value:0} / {unit.MaxHealth:0}";
            damageTaken.text = $"{unit.DamageTaken.Value:0}";
        }
    }
}