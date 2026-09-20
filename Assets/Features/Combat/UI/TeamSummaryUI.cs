using Common.Infrastructure;
using Features.Combat.Logic;
using Features.Localization.Data;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Combat.UI
{
    /// <summary>
    /// One teams summary card on the right of the combat UI.
    /// </summary>
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

        private Combatant _combatant;

        public void Bind(Combatant combatant, string partyName)
        {
            Unbind();
            _loc = ResourceManager.Instance.LocalizationResources;

            if (combatant == null)
                return;

            _combatant = combatant;

            partyNameText.text = partyName;
            tierIcon.sprite = ResourceManager.Instance.TierResources.GetTierIconByLevel(combatant.Level);

            unitCountRow.SetCount(combatant.AliveCount, combatant.UnitCount);
            combatStrengthRow.SetStat(combatant.UnitCombatStrength);
            healthRow.SetStat(combatant.UnitHealth);
            totalHealthRow.SetTotal(combatant.TotalHealth);
            totalCombatStrengthRow.SetTotal(combatant.TotalCombatStrength);

            SetRoundDeltas(CombatantDelta.None);
            Say(CombatMood.Start);
        }

        public void Unbind()
        {
            _combatant = null;
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

        private void OnDestroy()
        {
            Unbind();
        }
    }
}