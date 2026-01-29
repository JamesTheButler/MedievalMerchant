using System.Collections.Generic;
using System.Text;
using Common.Infrastructure.Observation;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Tutorial.UI
{
    public sealed class TutorialTaskListUI : MonoBehaviour
    {
        [SerializeField, Required]
        public TMP_Text taskText;

        private List<TutorialTask> _tasks;

        private readonly Bindings _bindings = new();

        public void SetUp(List<TutorialTask> tasks)
        {
            _tasks = tasks;
            RefreshTaskText();

            foreach (var task in _tasks)
            {
                _bindings.Track(
                    task.IsCompleted.Observe(RefreshTaskText, false)
                );
            }
        }

        private void OnDestroy()
        {
            _bindings.UnbindAll();
        }

        private void RefreshTaskText()
        {
            var tasksString = new StringBuilder();
            foreach (var task in _tasks)
            {
                var icon = task.IsCompleted.Value ? "x" : "-";
                tasksString.AppendLine($"{icon} {task.Message}");
            }

            taskText.text = tasksString.ToString();
        }
    }
}