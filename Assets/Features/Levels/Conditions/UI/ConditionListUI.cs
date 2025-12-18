using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Utility;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Model;
using UnityEngine;
using ConditionResources = Features.Levels.Conditions.Config.ConditionResources;

namespace Features.Levels.Conditions.UI
{
    public sealed class ConditionListUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject listItemPrefab;

        [SerializeField]
        private GameObject listContainer;

        private readonly Lazy<ConditionResources> _conditionResources =
            new(() => ResourceManager.Instance.ConditionResources);

        public void Setup(IEnumerable<ConditionData> conditionDatas)
        {
            Clear();

            foreach (var condition in conditionDatas)
            {
                var listItem = Instantiate(listItemPrefab, listContainer.transform);
                var listItemScript = listItem.GetComponent<ConditionListItem>();
                var icon = _conditionResources.Value.Conditions[condition.Type].Icon;

                listItemScript.Setup(condition.Description, icon);
            }
        }
        
        public void Setup(IEnumerable<ICondition> conditions)
        {
            Clear();

            foreach (var condition in conditions)
            {
                var listItem = Instantiate(listItemPrefab, listContainer.transform);
                var listItemScript = listItem.GetComponent<ConditionListItem>();
                var icon = _conditionResources.Value.Conditions[condition.Type].Icon;

                listItemScript.Setup(condition.Description, icon, condition.Progress);
                if (condition is not ILossCondition)
                    continue;

                var warningThreshold = _conditionResources.Value.WarningThresholdPercent;
                var warningIcon = _conditionResources.Value.WarningIcon;
                listItemScript.AddThreshold(warningThreshold, warningIcon);
            }
        }

        public void Clear()
        {
            listContainer.DestroyChildren();
        }
    }
}