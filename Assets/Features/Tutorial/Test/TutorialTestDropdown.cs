using System.Linq;
using Features.Tutorial.Data;
using Features.Tutorial.UI;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Tutorial.Test
{
    public class TutorialTestDropdown : MonoBehaviour
    {
        [SerializeField, Required]
        private TutorialResources tutorialResources;

        [SerializeField, Required]
        private TutorialUI tutorialUI;

        private TMP_Dropdown _dropdown;

        private void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();

            var topics = tutorialResources.Topics.Keys
                .Select(topic => topic.ToString())
                .ToList();

            _dropdown.ClearOptions();
            _dropdown.AddOptions(topics);

            _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

            tutorialUI.Open();
            tutorialUI.Setup(tutorialResources.Topics[TutorialTopic.Intro]);
        }

        private void OnDropdownValueChanged(int selectedIndex)
        {
            tutorialUI.Reset();
            tutorialUI.Open();
            tutorialUI.Setup(tutorialResources.Topics.ElementAt(selectedIndex).Value);
        }
    }
}