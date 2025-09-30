using System.Linq;
using Levels;
using Levels.Conditions;
using NaughtyAttributes;
using TMPro;
using UI.Conditions;
using UnityEngine;

namespace UI.StartMenu
{
    public sealed class LevelInfoBox : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text nameText;

        [SerializeField, Required]
        private ConditionListUI winConditionList;

        [SerializeField, Required]
        private ConditionListUI lossConditionList;

        public void Setup(LevelInfo levelInfo)
        {
            nameText.text = levelInfo.LevelName;
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