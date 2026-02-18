using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Global;
using Common.Utility;
using Features.Levels;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.UI;
using Features.Levels.GameModifiers.Effects.Data;
using Features.Levels.GameModifiers.UI;
using Features.Localization.Data;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Features.StartMenu.UI
{
    public sealed class LevelInfoBox : MonoBehaviour
    {
        [SerializeField, Required]
        private LevelLoader levelLoader;

        [SerializeField, Required]
        private AllySelectionPanel allySelectionPanel;

        [SerializeField, Required]
        private LocalizeStringEvent levelIdText, nameText, descriptionText, difficultyText;

        [SerializeField, Required]
        private TMP_Text completionDateText;

        [SerializeField, Required]
        private PreGameConditionListUI winConditionList, lossConditionList;

        [SerializeField, Required]
        private GameModifierUIElement levelConditionsElement;

        [SerializeField, Required]
        private Button continueButton, startButton;

        private AllyEffectData _allyEffect;
        private LevelInfo _currentLevelInfo;

        private LocalizationResources _localizationResources;

        private void Awake()
        {
            _localizationResources = ResourceManager.Instance.LocalizationResources;
            startButton.onClick.AddListener(StartButtonClicked);
            // relevant later, when I add serialization of ongoing games
            continueButton.gameObject.SetActive(false);
            allySelectionPanel.gameObject.SetActive(false);
        }

        public void Setup(LevelInfo levelInfo)
        {
            _currentLevelInfo = levelInfo;

            levelIdText.SetArguments(levelInfo.DisplayIndex);
            nameText.StringReference = levelInfo.LevelName;
            descriptionText.StringReference = levelInfo.Description;
            var completionDate = GlobalContext.Instance.Model.ProgressModel.CompletedLevels[levelInfo.InternalIndex];
            var isCompleted = completionDate != null;
            completionDateText.enabled = isCompleted;
            if (isCompleted)
            {
                completionDateText.text = $"Fastest Win: {completionDate!.CompletionDate.ToDisplayString()}";
            }

            // TODO: this used to be "Difficulty: <Difficulty>.WithColor(Difficulty.Color)"
            difficultyText.Update(_localizationResources.Difficulties[levelInfo.Difficulty]);

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