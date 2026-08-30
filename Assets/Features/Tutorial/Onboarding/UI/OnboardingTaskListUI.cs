using System.Collections.Generic;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using Common.Utility;
using Features.Tutorial.Onboarding.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Tutorial.Onboarding.UI
{
    public sealed class OnboardingTaskListUI : MonoBehaviour
    {
        [SerializeField, Required]
        private CanvasGroup canvasGroup;

        [SerializeField, Required]
        private RectTransform taskListContainer;

        [SerializeField, Required]
        private DefaultListItem listItemPrefab;

        [SerializeField, Required]
        private Sprite incompleteTaskIcon, completedTaskIcon;

        private readonly Bindings _bindings = new();
        private readonly Dictionary<OnboardingTask, DefaultListItem> _tasksAndItems = new();

        public void SetUp(IEnumerable<OnboardingTask> tasks)
        {
            foreach (var task in tasks)
            {
                var listItem = Instantiate(listItemPrefab, taskListContainer);
                _tasksAndItems.Add(task, listItem);

                listItem.SetText(task.Message);

                _bindings.Track(
                    task.IsCompleted.Observe(_ => UpdateTaskListItem(task))
                );
            }
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
        }

        public void Clear()
        {
            _bindings.Unbind();
            taskListContainer.DestroyChildren();
            _tasksAndItems.Clear();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
        }

        private void OnDestroy()
        {
            _bindings.Unbind();
        }

        private void UpdateTaskListItem(OnboardingTask task)
        {
            if (!_tasksAndItems.TryGetValue(task, out var listItem))
                return;

            listItem.SetIcon(task.IsCompleted.Value ? completedTaskIcon : incompleteTaskIcon);
            listItem.Text.fontStyle = task.IsCompleted.Value ? FontStyles.Strikethrough : FontStyles.Normal;
        }
    }
}