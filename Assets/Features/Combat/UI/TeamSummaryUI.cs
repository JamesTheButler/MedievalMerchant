using Common.Infrastructure;
using Features.Combat.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
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

        [SerializeField]
        private Sprite unitIcon, healthIcon, combatStrengthIcon;

        [SerializeField]
        private LocalizedString commanderLineString;

        private Combatant _combatant;

        public void Bind(Combatant combatant, string partyName)
        {
            Unbind();

            if (combatant == null)
                return;

            _combatant = combatant;

            partyNameText.text = partyName;
            tierIcon.sprite = ResourceManager.Instance.TierResources.GetTierIconByLevel(combatant.Level);

            unitCountRow.SetCount(unitIcon, combatant.AliveCount, combatant.UnitCount);
            combatStrengthRow.SetStat(combatStrengthIcon, combatant.UnitCombatStrength);
            healthRow.SetStat(healthIcon, combatant.UnitHealth);
            totalHealthRow.SetTotal(healthIcon, combatant.TotalHealth);
            totalCombatStrengthRow.SetTotal(combatStrengthIcon, combatant.TotalCombatStrength);

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
            var args = new
            {
                Line = mood.ToString(),
            };

            commanderLineText.text = commanderLineString.GetLocalizedString(args);
        }

        private void OnDestroy()
        {
            Unbind();
        }
    }
}