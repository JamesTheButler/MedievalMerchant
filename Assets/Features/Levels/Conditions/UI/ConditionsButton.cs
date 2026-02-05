using Common.Infrastructure.Gameplay;
using Features.Levels.Conditions.Model;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Levels.Conditions.UI
{
    public sealed class ConditionsButton : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text text;

        [SerializeField, Required]
        private Image warningIcon;

        private int _winConditionCount;
        private LevelConditions _levelConditions;

        private void Start()
        {
            _levelConditions = GameplayContext.Instance.Model.Conditions;

            _winConditionCount = _levelConditions.WinConditions.Count;
            _levelConditions.CompletionCountChanged.Observe(UpdateText);
            _levelConditions.IsLossClose.Observe(UpdateIcon);

            UpdateText(0);
            UpdateIcon(false);
        }

        private void OnDestroy()
        {
            _levelConditions.CompletionCountChanged.StopObserving(UpdateText);
            _levelConditions.IsLossClose.StopObserving(UpdateIcon);
        }

        private void UpdateText(int conditionCount)
        {
            text.text = $"{conditionCount}/{_winConditionCount}";
        }

        private void UpdateIcon(bool isLossClose)
        {
            warningIcon.gameObject.SetActive(isLossClose);
        }
    }
}