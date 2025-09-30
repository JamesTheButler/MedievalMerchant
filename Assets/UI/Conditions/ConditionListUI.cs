using System;
using System.Collections.Generic;
using Common;
using Data.Configuration;
using Levels.Conditions;
using UnityEngine;

namespace UI.Conditions
{
    public sealed class ConditionListUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject listItemPrefab;

        [SerializeField]
        private GameObject listContainer;

        private readonly Lazy<ConditionConfig> _conditionConfig =
            new(() => ConfigurationManager.Instance.ConditionConfig);

        public void Setup(IEnumerable<Condition> conditions, bool setupProgress)
        {
            Clear();

            foreach (var condition in conditions)
            {
                var listItem = Instantiate(listItemPrefab, listContainer.transform);
                var listItemScript = listItem.GetComponent<ConditionListItem>();
                var icon = _conditionConfig.Value.Conditions[condition.Type].Icon;

                listItemScript.Setup(condition.Description, icon, setupProgress ? condition.Progress : null);
                if (condition is not LossCondition)
                    continue;

                var warningThreshold = _conditionConfig.Value.WarningThresholdPercent;
                var warningIcon = _conditionConfig.Value.WarningIcon;
                listItemScript.AddThreshold(warningThreshold, warningIcon);
            }
        }

        public void Clear()
        {
            listContainer.DestroyChildren();
        }
    }
}