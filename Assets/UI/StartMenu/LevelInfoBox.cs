using System.Linq;
using Features.Levels.Config;
using Features.Levels.Config.Conditions;
using Infrastructure;
using NaughtyAttributes;
using TMPro;
using UI.Conditions;
using UnityEngine;

namespace UI.StartMenu
{
    public sealed class LevelInfoBox : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text nameText, completionDateText, descriptionText;

        [SerializeField, Required]
        private ConditionListUI winConditionList;

        [SerializeField, Required]
        private ConditionListUI lossConditionList;

        public void Setup(LevelInfo levelInfo)
        {
            nameText.text = levelInfo.LevelName;
            descriptionText.text = levelInfo.Description;
            var completionDate = GlobalContext.Instance.ProgressModel.CompletedLevels[levelInfo.InternalIndex];
            var isCompleted = completionDate != null;
            completionDateText.enabled = isCompleted;
            completionDateText.text = $"Fastest Win: {completionDate?.CompletionDate}";

            var conditions = levelInfo.Conditions;
            winConditionList.Setup(conditions.OfType<WinCondition>(), false);
            lossConditionList.Setup(conditions.OfType<LossCondition>(), false);
            gameObject.SetActive(true);
        }

        public void Clear()
        {
            nameText.text = string.Empty;
            gameObject.SetActive(false);
        }
    }
}