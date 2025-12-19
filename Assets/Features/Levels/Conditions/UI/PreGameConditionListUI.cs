using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Utility;
using Features.Levels.Conditions.Config;
using Features.Levels.Conditions.Data;
using UnityEngine;

namespace Features.Levels.Conditions.UI
{
    public sealed class PreGameConditionListUI : MonoBehaviour
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
                var listItemScript = listItem.GetComponent<PreGameConditionListItem>();
                var icon = _conditionResources.Value.Conditions[condition.Type].Icon;

                listItemScript.Setup(icon, condition.Description);
            }
        }

        private void Clear()
        {
            listContainer.DestroyChildren();
        }
    }
}