using System.Linq;
using Common.UI;
using Features.Levels.Config;
using Features.Levels.Config.Conditions;
using Infrastructure;
using NaughtyAttributes;
using TMPro;
using UI.Conditions;
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
        private TMP_Text nameText, completionDateText, descriptionText, difficultyText;

        [SerializeField, Required]
        private ConditionListUI winConditionList, lossConditionList;

        [SerializeField, Required]
        private Button continueButton, startButton;

        private LevelInfo _currentLevelInfo;

        public void Setup(LevelInfo levelInfo)
        {
            _currentLevelInfo = levelInfo;

            nameText.text = levelInfo.LevelName;
            descriptionText.text = levelInfo.Description;
            var completionDate = GlobalContext.Instance.ProgressModel.CompletedLevels[levelInfo.InternalIndex];
            var isCompleted = completionDate != null;
            completionDateText.enabled = isCompleted;
            completionDateText.text = $"Fastest Win: {completionDate?.CompletionDate}";
            difficultyText.text = $"Difficulty: {levelInfo.Difficulty.WithColor(levelInfo.DifficultyColor)}";

            var conditions = levelInfo.Conditions;
            winConditionList.Setup(conditions.OfType<WinCondition>(), false);
            lossConditionList.Setup(conditions.OfType<LossCondition>(), false);
            gameObject.SetActive(true);

            // relevant later, when i add serialization of ongoing games
            continueButton.gameObject.SetActive(false);
            startButton.onClick.AddListener(LoadCurrentLevel);
        }

        public void Clear()
        {
            nameText.text = string.Empty;
            gameObject.SetActive(false);
        }

        private void LoadCurrentLevel()
        {
            Debug.Log($"Loading level {_currentLevelInfo.LevelName}...");

            GlobalContext.CurrentLevelInfo = _currentLevelInfo;
            SceneManager.LoadScene(gameScene);
        }
    }
}