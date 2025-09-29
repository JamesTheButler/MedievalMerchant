using System;
using System.Collections.Generic;
using Common;
using Data.Configuration;
using Levels.Conditions;
using UnityEngine;

namespace UI
{
    public sealed class ConditionListUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject listItemPrefab;

        [SerializeField]
        private GameObject listContainer;

        private readonly Lazy<ConditionConfig> _conditionConfig =
            new(() => ConfigurationManager.Instance.ConditionConfig);

        public void Setup(IEnumerable<Condition> conditions)
        {
            Clear();

            foreach (var condition in conditions)
            {
                var listItem = Instantiate(listItemPrefab, listContainer.transform);
                var listItemScript = listItem.GetComponent<ConditionListItem>();

                var icon = _conditionConfig.Value.Conditions[condition.Type].Icon;
                listItemScript.Setup(condition.Description, "TBD", icon);
            }
        }

        public void Clear()
        {
            listContainer.DestroyChildren();
        }
    }
}