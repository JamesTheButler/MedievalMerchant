using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements;
using Features.Goods.Config;
using Features.Localization.Data;
using Features.Map.Modes;
using Features.Player.Caravan.Logic;
using Features.Player.Caravan.UI;
using Features.Ticking.Logic;
using Features.Towns;
using Features.Towns.Production.UI;
using Features.Towns.UI;
using Features.Trade;
using Features.Trade.UI;
using Features.Tutorial.Onboarding.Data;
using Features.Tutorial.Onboarding.Logic.Steps;
using Features.Tutorial.Onboarding.UI;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace Features.Tutorial.Onboarding.Logic
{
    public sealed class OnboardingController : InitializableBehavior
    {
        [field: SerializeField, Required]
        public CaravanInventoryUI CartInventoryUI { get; private set; }

        [field: SerializeField, Required]
        public TownUI TownUI { get; private set; }

        [field: SerializeField, Required]
        public TownUIProductionSection TownProducerUI { get; private set; }

        [field: SerializeField, Required]
        public TradeUI TradeUI { get; private set; }

        [SerializeField, Required]
        private OnboardingExplainerUI explainerUI;

        [SerializeField, Required]
        private OnboardingUIBlinker uiBlinker;

        [SerializeField, Required]
        private OnboardingMapBlinker mapBlinker;

        [SerializeField, Required]
        private OnboardingTaskListUI taskListUI;

        [SerializeField]
        private float delayBetweenSteps = 0.5f;

        private Coroutine _tutorialCoroutine;

        private OnboardingResources _onboardingResources;
        private OnboardingLocalizationResources _localizationResources;
        private GoodResources _goodResources;

        private MapModeModel _mapModeModel;
        private GameSpeedModel _gameSpeedModel;
        private CaravanManager _caravanManager;

        private Town _townA, _townB;
        private OnboardingSequence _onboardingSequence;

        private object _townNameObject;

        private const int BerryDeliveryCount = 30;

        public override void Initialize()
        {
            var model = GameplayContext.Instance.Model;
            _mapModeModel = model.MapModeModel;
            _gameSpeedModel = model.GameSpeed;
            _caravanManager = model.Player.CaravanManager;

            var resourceManager = ResourceManager.Instance;
            _onboardingResources = resourceManager.OnboardingResources;
            _localizationResources = resourceManager.LocalizationResources.Onboarding;
            _goodResources = resourceManager.GoodResources;

            _townA = model.Towns.Values.ElementAt(0);
            _townB = model.Towns.Values.ElementAt(1);

            _townNameObject = new { TownA = _townA.Name, TownB = _townB.Name };

            _onboardingSequence = new OnboardingSequence(0f,
                HayDeliverySequence(),
                BerryPickerSequence(),
                GameSpeedControlsSequence(),
                BerryDeliverySequence(),
                FinishOnboardingSequence()
            );

            HideBlinker();
        }

        public void StartTutorial()
        {
            if (_tutorialCoroutine != null)
            {
                StopCoroutine(_tutorialCoroutine);
            }

            _tutorialCoroutine = StartCoroutine(_onboardingSequence.Run(this));
        }

        public void PostExplainer(OnboardingExplainer explainer, Action onNextClicked)
        {
            var message = _onboardingResources.explainerTexts.GetValueOrDefault(explainer, null);
            if (message == null)
                return;

            explainerUI.Show(message.GetLocalizedString(_townNameObject), onNextClicked);
        }

        public void HideExplainer()
        {
            explainerUI.Hide();
        }

        public void Blink(MonoBehaviour uiElement, MouseButton mouseButton)
        {
            mapBlinker.Hide();
            uiBlinker.Show(uiElement, mouseButton);
        }

        public void Blink(Town town, MouseButton mouseButton)
        {
            uiBlinker.Hide();
            mapBlinker.Show(town.WorldLocation + new Vector2(.5f, .5f), mouseButton);
        }

        public void HideBlinker()
        {
            mapBlinker.Hide();
            uiBlinker.Hide();
        }

        public void AddTasks(List<OnboardingTask> tasks)
        {
            taskListUI.SetUp(tasks);
        }

        public void ClearTasks()
        {
            taskListUI.Clear();
        }

        #region Sequences

        private OnboardingSequence HayDeliverySequence()
        {
            var loc = _localizationResources;
            var goodName = _goodResources.ResourceData[Good.T1Hay].GoodName;
            const int hayAmount = 15;

            var buyHayTask = new OnboardingTask(loc.BuyGoodsTask(hayAmount, goodName, _townA.Name));
            var goToATask = new OnboardingTask(loc.TravelToTask(_townA.Name));
            var goToBTask = new OnboardingTask(loc.TravelToTask(_townB.Name));
            var sellHayTask = new OnboardingTask(loc.SellGoodsTask(hayAmount, goodName, _townA.Name));

            return new OnboardingSequence(delayBetweenSteps,
                new OnboardingSimpleStep(() => { _gameSpeedModel.Pause(); }),
                new OnboardingExplainerStep(OnboardingExplainer.Welcome),
                new OnboardingExplainerStep(OnboardingExplainer.IntroGoal),
                new OnboardingSimpleStep(() => { _mapModeModel.MapMode.Value = MapMode.Town; }),
                new OnboardingExplainerStep(OnboardingExplainer.IntroCampsite),
                new OnboardingTaskStep(goToATask),
                new OnboardingTravelStep(_townA, goToATask),
                new OnboardingTaskClearStep(),
                new OnboardingExplainerStep(OnboardingExplainer.HayMissionInstructions),
                new OnboardingTaskStep(buyHayTask, goToBTask, sellHayTask),
                new OnboardingSimpleStep(() => { _gameSpeedModel.Resume(); }),
                new OnboardingTradeStep(TradeType.Buy, Good.T1Hay, hayAmount, _townA, buyHayTask),
                new OnboardingSimpleStep(() => { TownUI.Close(); }),
                new OnboardingTravelStep(_townB, goToBTask),
                new OnboardingTradeStep(TradeType.Sell, Good.T1Hay, hayAmount, _townB, sellHayTask),
                new OnboardingTaskClearStep()
            );
        }

        private OnboardingSequence BerryPickerSequence()
        {
            var producerName = _goodResources.ResourceData[Good.T1Berries].BuildingName;

            var taskString = _localizationResources.BuildProducerTask(_townB.Name, producerName);
            var buildBerryPickerTask = new OnboardingTask(taskString);

            var buildBerryPickerSequence = new OnboardingSequence(delayBetweenSteps,
                new OnboardingSimpleStep(() => { _gameSpeedModel.Pause(); }),
                new OnboardingExplainerStep(OnboardingExplainer.HayMissionComplete),
                new OnboardingEnsureFundsStep(505),
                new OnboardingSimpleStep(() => { _townA.DevelopmentManager.AddDevelopmentChange(100); }),
                new OnboardingExplainerStep(OnboardingExplainer.TownAUpgradeReady),
                new OnboardingExplainerStep(OnboardingExplainer.BerryPickerInstructions),
                new OnboardingTaskStep(buildBerryPickerTask),
                new OnboardingSimpleStep(() => { _gameSpeedModel.Resume(); }),
                new OnboardingBuildProducerStep(_townB, Good.T1Berries, buildBerryPickerTask),
                new OnboardingEnsureInventoryStep(_townB, Good.T1Berries, 20),
                new OnboardingTaskClearStep()
            );
            return buildBerryPickerSequence;
        }

        private OnboardingSequence GameSpeedControlsSequence()
        {
            var berryName = _goodResources.ResourceData[Good.T1Berries].GoodName;
            var pauseGameTask = new OnboardingTask(_localizationResources.UnpauseGameTask());
            var speedUpGameTask = new OnboardingTask(_localizationResources.SetSpeedTask());
            var waitForBerriesText = _localizationResources.WaitForBerriesTask(
                BerryDeliveryCount,
                berryName,
                _townB.Name);
            var waitForBerriesTask = new OnboardingTask(waitForBerriesText);

            return new OnboardingSequence(delayBetweenSteps,
                new OnboardingSimpleStep(() => { _gameSpeedModel.Pause(); }),
                new OnboardingExplainerStep(OnboardingExplainer.BerryPickerComplete),
                new OnboardingExplainerStep(OnboardingExplainer.GameSpeedInstructions),
                new OnboardingTaskStep(pauseGameTask, speedUpGameTask, waitForBerriesTask),
                new OnboardingResumeGameStep(pauseGameTask),
                new OnboardingSetGameSpeedTask(speedUpGameTask),
                new OnboardingWaitUntilStep(() => _townB.Inventory.HasGood(Good.T1Berries, BerryDeliveryCount)),
                new OnboardingTaskClearStep()
            );
        }

        private OnboardingSequence BerryDeliverySequence()
        {
            var loc = _localizationResources;

            const int gameCount = 20;
            var berryName = _goodResources.ResourceData[Good.T1Berries].GoodName;
            var gameName = _goodResources.ResourceData[Good.T1WildGame].GoodName;

            var buyBerriesTask = new OnboardingTask(loc.BuyGoodsTask(BerryDeliveryCount, berryName, _townB.Name));
            var buyGameTask = new OnboardingTask(loc.BuyGoodsTask(gameCount, gameName, _townB.Name));
            var goToATask = new OnboardingTask(loc.TravelToTask(_townA.Name));
            var sellBerriesTask = new OnboardingTask(loc.SellGoodsTask(BerryDeliveryCount, berryName, _townA.Name));
            var sellGameTask = new OnboardingTask(loc.SellGoodsTask(gameCount, gameName, _townA.Name));

            return new OnboardingSequence(delayBetweenSteps,
                new OnboardingSimpleStep(() => { _gameSpeedModel.Pause(); }),
                new OnboardingExplainerStep(OnboardingExplainer.BerryDeliveryInstructions),
                new OnboardingTaskStep(
                    buyBerriesTask,
                    buyGameTask,
                    goToATask,
                    sellBerriesTask,
                    sellGameTask),
                new OnboardingSimpleStep(() => { _caravanManager.UpgradeCart(0); }),
                new OnboardingEnsureFundsStep(300),
                new OnboardingTradeStep(TradeType.Buy, Good.T1Berries, BerryDeliveryCount, _townB, buyBerriesTask),
                new OnboardingEnsureFundsStep(300),
                new OnboardingTradeStep(TradeType.Buy, Good.T1WildGame, gameCount, _townB, buyGameTask),
                new OnboardingTravelStep(_townA, goToATask),
                new OnboardingTradeStep(TradeType.Sell, Good.T1Berries, BerryDeliveryCount, _townA, sellBerriesTask),
                new OnboardingTradeStep(TradeType.Sell, Good.T1WildGame, gameCount, _townA, sellGameTask),
                new OnboardingTaskClearStep()
            );
        }

        private OnboardingSequence FinishOnboardingSequence()
        {
            const float townBDevelopmentLevel = 87.5f;
            var townUpgradeTask = new OnboardingTask(_localizationResources.UpgradeTownTask(_townB.Name, Tier.Tier2));

            return new OnboardingSequence(delayBetweenSteps,
                new OnboardingSimpleStep(() => { _gameSpeedModel.Pause(); }),
                new OnboardingExplainerStep(OnboardingExplainer.TownAUpgraded),
                new OnboardingExplainerStep(OnboardingExplainer.FindYourOwnFortune),
                new OnboardingExplainerStep(OnboardingExplainer.ClosingRemarks),
                new OnboardingSimpleStep(() => { _gameSpeedModel.Resume(); }),
                new OnboardingSimpleStep(() =>
                {
                    var currentDev = _townB.DevelopmentManager.DevelopmentScore.Value;
                    if (currentDev > townBDevelopmentLevel)
                        return;

                    _townB.DevelopmentManager.AddDevelopmentChange(townBDevelopmentLevel - currentDev);
                }),
                new OnboardingEnsureFundsStep(800),
                new OnboardingTaskStep(townUpgradeTask),
                new OnboardingTownUpgradeStep(_townB, Tier.Tier2, townUpgradeTask),
                new OnboardingTaskClearStep()
            );
        }

        #endregion
    }
}