using Common;
using Features.Levels.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Conditions
{
    public class ConditionsButton : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text text;

        [SerializeField, Required]
        private Image warningIcon;

        private int _winConditionCount;
        private ConditionManager _conditionManager;

        private void Start()
        {
            _conditionManager = Model.Instance.ConditionManager;

            _winConditionCount = _conditionManager.WinConditions.Count;
            _conditionManager.CompletionCountChanged += UpdateText;
            _conditionManager.IsLossClose.Observe(UpdateIcon);

            UpdateText(0);
            UpdateIcon(false);
        }

        private void OnDestroy()
        {
            _conditionManager.CompletionCountChanged -= UpdateText;
            _conditionManager.IsLossClose.StopObserving(UpdateIcon);
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