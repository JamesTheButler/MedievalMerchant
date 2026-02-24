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
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Features.StartMenu.UI
{
    public sealed class LevelInfoBox : MonoBehaviour
    {
        [SerializeField, Required]
        private LevelLoader levelLoader;

        [SerializeField, Required]
        private AllySelectionPanel allySelectionPanel;

        [SerializeField]
        private LocalizedString bestTimeString, levelIndexString;

        [SerializeField, Required]
        private TMP_Text difficultyText, bestTimeText, levelIdText, nameText, descriptionText;

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

            levelIdText.text = levelIndexString.GetLocalizedString(new { _int_LevelIndex = levelInfo.DisplayIndex });
            nameText.text = levelInfo.LevelName.GetLocalizedString();
            descriptionText.text = levelInfo.Description.GetLocalizedString();
            var completionDate = GlobalContext.Instance.Model.ProgressModel.CompletedLevels[levelInfo.InternalIndex];
            var isCompleted = completionDate != null;
            bestTimeText.gameObject.SetActive(isCompleted);
            if (isCompleted)
            {
                bestTimeText.text = bestTimeString.GetLocalizedString(new
                {
                    _int_Day = completionDate!.CompletionDate.Day,
                    _int_Year = completionDate!.CompletionDate.Year,
                });
            }

            difficultyText.SetLocalizedText(_localizationResources.Difficulties[levelInfo.Difficulty]);

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