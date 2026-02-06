using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using Common.Utility;
using Features.Levels.Conditions.Model;
using NaughtyAttributes;
using UnityEngine;
using ConditionResources = Features.Levels.Conditions.Config.ConditionResources;

namespace Features.Levels.Conditions.UI
{
    public sealed class InGameConditionListUI : InitializableBehavior
    {
        [SerializeField, Required]
        private InGameConditionListItem listItemPrefab;

        [SerializeField, Required]
        private GameObject listContainer;

        [SerializeField, Required]
        private Sprite incompleteIcon, warningIcon, completeIcon;

        private ConditionResources _conditionResources;

        private readonly Dictionary<ICondition, InGameConditionListItem> _listItems = new();

        private readonly Bindings _bindings = new();

        public override void Initialize()
        {
            _conditionResources = ResourceManager.Instance.ConditionResources;
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _bindings.UnbindAll();
        }

        public void Setup(IEnumerable<ICondition> conditions)
        {
            Clear();

            foreach (var condition in conditions)
            {
                var listItem = Instantiate(listItemPrefab, listContainer.transform);
                var icon = _conditionResources.Conditions[condition.Type].Icon;
                listItem.Setup(condition.Description, icon);
                _listItems.Add(condition, listItem);

                _bindings.Track(
                    condition.Progress.CurrentValueText.Observe(listItem.SetProgressText),
                    condition.Progress.IsCompleted.Observe(isCompleted =>
                        listItem.SetProgressIcon(isCompleted ? completeIcon : incompleteIcon))
                );

                if (condition is not ILossCondition lossCondition)
                    continue;

                _bindings.Track(
                    lossCondition.IsClose.Observe(isClose => RefreshProgressIcon(condition, isClose))
                );
            }
        }

        private void RefreshProgressIcon(ICondition condition, bool isClose)
        {
            var listItem = _listItems[condition];

            listItem.SetProgressIcon(condition.Progress.IsCompleted.Value
                ? completeIcon
                : isClose ? warningIcon : incompleteIcon);
        }

        private void Clear()
        {
            listContainer.DestroyChildren();
        }
    }
}