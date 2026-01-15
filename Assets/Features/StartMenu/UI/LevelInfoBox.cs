using System.Linq;
using Common.Infrastructure.Global;
using Common.UI.Utility;
using Features.Levels;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.UI;
using Features.Levels.GameModifiers.UI;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.StartMenu.UI
{
    public sealed class LevelInfoBox : MonoBehaviour
    {
        [SerializeField, Scene]
        private string gameScene;

        [SerializeField, Required]
        private TMP_Text levelIdText, nameText, completionDateText, descriptionText, difficultyText;

        [SerializeField, Required]
        private PreGameConditionListUI winConditionList;

        [SerializeField, Required]
        private PreGameConditionListUI lossConditionList;

        [SerializeField, Required]
        private GameModifierUIElement levelConditionsElement;

        [SerializeField, Required]
        private Button continueButton, startButton;

        private LevelInfo _currentLevelInfo;

        private void Awake()
        {
            startButton.onClick.AddListener(LoadCurrentLevel);
            // relevant later, when I add serialization of ongoing games
            continueButton.gameObject.SetActive(false);
        }

        public void Setup(LevelInfo levelInfo)
        {
            _currentLevelInfo = levelInfo;

            levelIdText.text = levelInfo.LevelNumberText;
            nameText.text = levelInfo.LevelName;
            descriptionText.text = levelInfo.Description;
            var completionDate = GlobalContext.Instance.Model.ProgressModel.CompletedLevels[levelInfo.InternalIndex];
            var isCompleted = completionDate != null;
            completionDateText.enabled = isCompleted;
            if (isCompleted)
                completionDateText.text = $"Fastest Win: {completionDate!.CompletionDate.ToDisplayString()}";

            difficultyText.text = $"Difficulty: {levelInfo.Difficulty.WithColor(levelInfo.DifficultyColor)}";

            var conditions = levelInfo.Conditions;
            winConditionList.Setup(conditions.OfType<WinConditionData>());
            lossConditionList.Setup(conditions.OfType<LossConditionData>());
            levelConditionsElement.Setup(levelInfo.GameplayModifiers);
        }

        private void LoadCurrentLevel()
        {
            Debug.Log($"Loading level {_currentLevelInfo.LevelName}...");

            GlobalContext.CurrentLevelInfo = _currentLevelInfo;
            SceneManager.LoadScene(gameScene);
        }
    }
}