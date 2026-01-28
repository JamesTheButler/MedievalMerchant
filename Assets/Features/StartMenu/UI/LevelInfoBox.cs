using System.Linq;
using Common.Infrastructure.Global;
using Common.UI.Utility;
using Common.Utility;
using Features.Levels;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.UI;
using Features.Levels.GameModifiers.Effects.Data;
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
        [SerializeField, Required]
        private LevelLoader levelLoader;

        [SerializeField, Required]
        private TMP_Text levelIdText, nameText, completionDateText, descriptionText, difficultyText;

        [SerializeField, Required]
        private PreGameConditionListUI winConditionList, lossConditionList;

        [SerializeField, Required]
        private GameModifierUIElement levelConditionsElement;

        [SerializeField, Required]
        private AllySelectionPanel allySelectionPanel;

        [SerializeField, Required]
        private Button continueButton, startButton;

        private AllyEffectData _allyEffect;
        private LevelInfo _currentLevelInfo;

        private void Awake()
        {
            startButton.onClick.AddListener(StartButtonClicked);
            // relevant later, when I add serialization of ongoing games
            continueButton.gameObject.SetActive(false);
            allySelectionPanel.gameObject.SetActive(false);
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
            {
                completionDateText.text = $"Fastest Win: {completionDate!.CompletionDate.ToDisplayString()}";
            }

            difficultyText.text = $"Difficulty: {levelInfo.Difficulty.WithColor(levelInfo.DifficultyColor)}";

            var conditions = levelInfo.Conditions;
            winConditionList.Setup(conditions.OfType<WinConditionData>());
            lossConditionList.Setup(conditions.OfType<LossConditionData>());
            levelConditionsElement.Setup(levelInfo.GameplayModifiers);

            _allyEffect = levelInfo.GameplayModifiers.Effects.FirstOfType<AllyEffectData, EffectData>();
        }

        private void StartButtonClicked()
        {
            if (_allyEffect != null)
            {
                allySelectionPanel.SetUp(_allyEffect, LoadSelectedLevel);
                return;
            }

            LoadSelectedLevel();
        }

        private void LoadSelectedLevel()
        {
            levelLoader.LoadLevel(_currentLevelInfo);
        }
    }
}