using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements;
using Features.Map.Modes;
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
        public CaravanPanelUI CaravanPanelUI { get; private set; }

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

        private const float DelayBetweenSteps = 0.5f;

        private Coroutine _tutorialCoroutine;
        private OnboardingResources _onboardingResources;
        private MapModeModel _mapModeModel;
        private GameSpeedModel _gameSpeedModel;

        private Town _townA, _townB;
        private OnboardingSequence _onboardingSequence;

        public override void Initialize()
        {
            var model = GameplayContext.Instance.Model;
            _mapModeModel = model.MapModeModel;
            _gameSpeedModel = model.GameSpeed;

            _townA = model.Towns.Values.ElementAt(0);
            _townB = model.Towns.Values.ElementAt(1);

            _onboardingResources = ResourceManager.Instance.OnboardingResources;

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

        public void PostExplainer(int explainerIndex, Action onNextClicked)
        {
            var message = _onboardingResources.explainerTexts
                .GetValueOrDefault(explainerIndex, string.Empty)
                .Replace("TownA", _townA.Name)
                .Replace("TownB", _townB.Name);

            explainerUI.Show(message, onNextClicked);
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
            var buyHayTask = new OnboardingTask($"Buy 15 Hay in {_townA.Name}");
            var goToATask = new OnboardingTask($"Travel to {_townB.Name}");
            var sellHayTask = new OnboardingTask($"Sell 15 Hay in {_townB.Name}");

            return new OnboardingSequence(DelayBetweenSteps,
                new OnboardingSimpleStep(() => { _gameSpeedModel.Pause(); }),
                new OnboardingExplainerStep(0),
                new OnboardingExplainerStep(1),
                new OnboardingSimpleStep(() => { _mapModeModel.MapMode.Value = MapMode.Town; }),
                new OnboardingExplainerStep(2),
                new OnboardingTaskStep(buyHayTask, goToATask, sellHayTask),
                new OnboardingSimpleStep(() => { _gameSpeedModel.Resume(); }),
                new OnboardingTradeStep(TradeType.Buy, Good.T1Hay, 15, _townA, buyHayTask),
                new OnboardingSimpleStep(() => { TownUI.Close(); }),
                new OnboardingTravelStep(_townB, goToATask),
                new OnboardingTradeStep(TradeType.Sell, Good.T1Hay, 15, _townB, sellHayTask),
                new OnboardingTaskClearStep()
            );
        }

        private OnboardingSequence BerryPickerSequence()
        {
            var buildBerryPickerTask = new OnboardingTask($"Build berry picker in {_townB.Name}");

            var buildBerryPickerSequence = new OnboardingSequence(DelayBetweenSteps,
                new OnboardingSimpleStep(() => { _gameSpeedModel.Pause(); }),
                new OnboardingExplainerStep(3),
                new OnboardingEnsureFundsStep(505),
                new OnboardingSimpleStep(() => { _townA.DevelopmentManager.AddDevelopmentChange(100); }),
                new OnboardingExplainerStep(4),
                new OnboardingExplainerStep(5),
                new OnboardingTaskStep(buildBerryPickerTask),
                new OnboardingSimpleStep(() => { _gameSpeedModel.Resume(); }),
                new OnboardingBuildProducerStep(_townB, Good.T1Berries, buildBerryPickerTask),
                new OnboardingSimpleStep(() =>
                {
                    var berryCount = _townB.Inventory.Get(Good.T1Berries);
                    _townB.Inventory.AddGood(Good.T1Berries, 20 - berryCount);
                }),
                new OnboardingTaskClearStep()
            );
            return buildBerryPickerSequence;
        }

        private OnboardingSequence GameSpeedControlsSequence()
        {
            var pauseGameTask = new OnboardingTask("Un-pause the game [Space]");
            var speedUpGameTask = new OnboardingTask("Set the game speed to fast [2]");

            return new OnboardingSequence(DelayBetweenSteps,
                new OnboardingSimpleStep(() => { _gameSpeedModel.Pause(); }),
                new OnboardingExplainerStep(6),
                new OnboardingExplainerStep(7),
                new OnboardingTaskStep(pauseGameTask, speedUpGameTask),
                new OnboardingResumeGameStep(pauseGameTask),
                new OnboardingSetGameSpeedTask(speedUpGameTask),
                new OnboardingTaskClearStep()
            );
        }

        private OnboardingSequence BerryDeliverySequence()
        {
            const int berryCount = 30;
            const int gameCount = 20;
            var upgradeCartTask = new OnboardingTask("Upgrade your cart to tier II.");
            var buyBerriesTask = new OnboardingTask($"Buy {berryCount} berries in {_townB.Name}.");
            var buyGameTask = new OnboardingTask($"Buy {gameCount} wild game in {_townB.Name}.");
            var goToATask = new OnboardingTask($"Travel to {_townA.Name}.");
            var sellBerriesTask = new OnboardingTask($"Sell 30 berries in {_townA.Name}.");
            var sellGameTask = new OnboardingTask($"Sell 20 wild game in {_townA.Name}.");

            return new OnboardingSequence(DelayBetweenSteps,
                new OnboardingWaitUntilStep(() => _townB.Inventory.HasGood(Good.T1Berries, berryCount)),
                new OnboardingSimpleStep(() => { _gameSpeedModel.Pause(); }),
                new OnboardingExplainerStep(8),
                new OnboardingExplainerStep(9),
                new OnboardingTaskStep(
                    upgradeCartTask,
                    buyBerriesTask,
                    buyGameTask,
                    goToATask,
                    sellBerriesTask,
                    sellGameTask),
                new OnboardingEnsureFundsStep(800),
                new OnboardingCartUpgradeStep(2, upgradeCartTask),
                new OnboardingEnsureFundsStep(300),
                new OnboardingTradeStep(TradeType.Buy, Good.T1Berries, berryCount, _townB, buyBerriesTask),
                new OnboardingEnsureFundsStep(300),
                new OnboardingTradeStep(TradeType.Buy, Good.T1WildGame, gameCount, _townB, buyGameTask),
                new OnboardingTravelStep(_townA, goToATask),
                new OnboardingTradeStep(TradeType.Sell, Good.T1Berries, berryCount, _townA, sellBerriesTask),
                new OnboardingTradeStep(TradeType.Sell, Good.T1WildGame, gameCount, _townA, sellGameTask),
                new OnboardingTaskClearStep()
            );
        }

        private OnboardingSequence FinishOnboardingSequence()
        {
            const float townBDevelopmentLevel = 87.5f;
            var townUpgradeTask = new OnboardingTask($"Upgrade {_townB.Name} to tier II.");

            return new OnboardingSequence(DelayBetweenSteps,
                new OnboardingSimpleStep(() => { _gameSpeedModel.Pause(); }),
                new OnboardingExplainerStep(10),
                new OnboardingExplainerStep(11),
                new OnboardingExplainerStep(12),
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