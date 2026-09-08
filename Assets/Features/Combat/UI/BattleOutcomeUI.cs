using System;
using Common.Infrastructure.Observation;
using Features.Combat.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Features.Combat.UI
{
    public sealed class BattleOutcomeUI : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text roundLabelText, titleText, detailText;

        [SerializeField, Required]
        private OutcomeLossTile playerLossTile, banditLossTile;

        [SerializeField, Required]
        private Button nextButton;

        [SerializeField]
        private LocalizedString roundLabelString, playerSideHeader, banditSideHeader;

        [SerializeField]
        private LocalizedString victoryTitle, defeatTitle, drawTitle;

        [SerializeField]
        private LocalizedString victoryDetail, defeatDetail, drawDetail;

        public ObservableEvent NextRequested { get; } = new();

        private void Awake()
        {
            nextButton.onClick.AddListener(OnNextClicked);
        }

        public void Show(CombatStatus status, Combatant player, Combatant bandits, int round)
        {
            if (status == CombatStatus.Ongoing)
            {
                Debug.LogError("BattleOutcomeUI.Show called while the battle is still ongoing.");
                return;
            }

            var roundArgs = new
            {
                _int_Round = round,
            };

            roundLabelText.text = roundLabelString.GetLocalizedString(roundArgs);
            titleText.text = TitleFor(status);
            detailText.text = DetailFor(status);

            SetLosses(playerLossTile, playerSideHeader.GetLocalizedString(), player, isPositiveGood: false);
            SetLosses(banditLossTile, banditSideHeader.GetLocalizedString(), bandits, isPositiveGood: true);

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private static void SetLosses(
            OutcomeLossTile tile,
            string header,
            Combatant combatant,
            bool isPositiveGood)
        {
            if (combatant == null)
                return;

            tile.SetLosses(
                header,
                combatant.UnitCount - combatant.AliveCount.Value,
                combatant.UnitCount,
                isPositiveGood);
        }

        private string TitleFor(CombatStatus status)
        {
            return status switch
            {
                CombatStatus.Victory => victoryTitle.GetLocalizedString(),
                CombatStatus.Defeat => defeatTitle.GetLocalizedString(),
                _ => drawTitle.GetLocalizedString(),
            };
        }

        private string DetailFor(CombatStatus status)
        {
            return status switch
            {
                CombatStatus.Victory => victoryDetail.GetLocalizedString(),
                CombatStatus.Defeat => defeatDetail.GetLocalizedString(),
                _ => drawDetail.GetLocalizedString(),
            };
        }

        private void OnNextClicked()
        {
            NextRequested?.Invoke();
        }

        private void OnDestroy()
        {
            nextButton.onClick.RemoveListener(OnNextClicked);
        }
    }
}