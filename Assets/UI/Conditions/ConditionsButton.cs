using Features.Levels.Logic;
using Infrastructure;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Conditions
{
    public sealed class ConditionsButton : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text text;

        [SerializeField, Required]
        private Image warningIcon;

        private int _winConditionCount;
        private LevelConditionManager _levelConditionManager;

        private void Start()
        {
            _levelConditionManager = GameplayContext.Instance.Systems.LevelConditionManager;

            _winConditionCount = _levelConditionManager.WinConditions.Count;
            _levelConditionManager.CompletionCountChanged += UpdateText;
            _levelConditionManager.IsLossClose.Observe(UpdateIcon);

            UpdateText(0);
            UpdateIcon(false);
        }

        private void OnDestroy()
        {
            _levelConditionManager.CompletionCountChanged -= UpdateText;
            _levelConditionManager.IsLossClose.StopObserving(UpdateIcon);
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