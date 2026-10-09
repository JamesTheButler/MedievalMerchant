using Common.Infrastructure;
using Features.Combat.Logic;
using Features.Localization.Data;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Combat.UI
{
    public sealed class TeamSummaryUI : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text partyNameText, commanderLineText;

        [SerializeField, Required]
        private Image tierIcon;

        [SerializeField, Required]
        private UnitCountRow unitCountRow;

        [SerializeField, Required]
        private ModifiedStatRow combatStrengthRow, healthRow;

        [SerializeField, Required]
        private TotalStatRow totalHealthRow, totalCombatStrengthRow;

        private LocalizationResources _loc;

        public void Bind(Combatant combatant, string partyName)
        {
            _loc = ResourceManager.Instance.LocalizationResources;

            if (combatant == null)
                return;

            partyNameText.text = partyName;
            tierIcon.sprite = ResourceManager.Instance.TierResources.GetTierIconByLevel(combatant.Level);

            unitCountRow.SetCount(combatant.AliveCount, combatant.UnitCount);
            combatStrengthRow.SetStat(combatant.UnitCombatStrength);
            healthRow.SetStat(combatant.UnitHealth);
            totalHealthRow.SetTotal(combatant.TotalHealth, _loc.Combat.TotalHealth);
            totalCombatStrengthRow.SetTotal(combatant.TotalCombatStrength, _loc.Combat.TotalStrength);

            SetRoundDeltas(CombatantDelta.None);
            Say(CombatMood.Start);
        }

        public void SetRoundDeltas(CombatantDelta delta)
        {
            if (delta == null)
                return;

            unitCountRow.SetDelta(delta.UnitsLost);
            totalHealthRow.SetDelta(delta.HealthLost);
            totalCombatStrengthRow.SetDelta(delta.CombatStrengthLost);
        }

        // TODO: placeholder. support blips
        public void Say(CombatMood mood)
        {
            commanderLineText.text = _loc.Quote(mood.ToString());
        }
    }
}