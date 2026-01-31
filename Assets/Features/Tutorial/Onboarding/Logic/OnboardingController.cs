using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements;
using Features.Player.Caravan.UI;
using Features.Player.Logic;
using Features.Towns;
using Features.Towns.Production.UI;
using Features.Trade;
using Features.Trade.UI;
using Features.Tutorial.Onboarding.Data;
using Features.Tutorial.Onboarding.Logic.Steps;
using Features.Tutorial.Onboarding.UI;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic
{
    public sealed class OnboardingController : InitializableBehavior
    {
        [SerializeField, Required]
        private OnboardingExplainerUI explainerUI;

        [SerializeField, Required]
        private OnboardingUIBlinker uiBlinker;

        [SerializeField, Required]
        private OnboardingMapBlinker mapBlinker;

        [SerializeField, Required]
        private OnboardingTaskListUI taskListUI;

        [SerializeField, Required]
        private CaravanPanelUI caravanPanelUI;

        [SerializeField, Required]
        private TownUIProductionSection townProducerUI;

        [SerializeField, Required]
        private TradeUI tradeUI;

        private Coroutine _tutorialCoroutine;

        private OnboardingResources _onboardingResources;
        private PlayerModel _player;
        private Town _townA, _townB;
        private OnboardingSequence _onboardingSequence;

        public override void Initialize()
        {
            var model = GameplayContext.Instance.Model;
            _player = model.Player;
            _townA = model.Towns.Values.ElementAt(0);
            _townB = model.Towns.Values.ElementAt(1);

            _onboardingResources = ResourceManager.Instance.OnboardingResources;

            _onboardingSequence = new OnboardingSequence(
                IntroSequence(),
                HayDeliverySequence(),
                BerryPickerSequence(),
                GameSpeedControlsSequence(),
                BerryDeliverySequence(),
                FinishOnboardingSequence()
            );
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
                .GetValueOrDefault(explainerIndex, "Error")
                .Replace("Town A", _townA.Name, StringComparison.InvariantCultureIgnoreCase)
                .Replace("TownA", _townA.Name, StringComparison.InvariantCultureIgnoreCase)
                .Replace("Town B", _townB.Name, StringComparison.InvariantCultureIgnoreCase)
                .Replace("TownB", _townB.Name, StringComparison.InvariantCultureIgnoreCase);

            explainerUI.Show(message, onNextClicked);
        }

        public void HideExplainer()
        {
            explainerUI.Hide();
        }

        public void BlinkPlayerInventoryCell(Good good)
        {
            var cell = caravanPanelUI
                .GetComponentsInChildren<GoodCell>()
                .FirstOrDefault(cell => cell.Good == good);

            if (cell == null)
                return;

            Blink(cell.GetComponent<RectTransform>());
        }

        public void BlinkTownProducerCell(Good good)
        {
            var cell = townProducerUI
                .GetComponentsInChildren<ProductionCell>()
                .FirstOrDefault(cell => cell.Good == good);

            if (cell == null)
                return;

            Blink(cell.GetComponent<RectTransform>());
        }

        private void Blink(RectTransform targetTransform)
        {
            mapBlinker.Hide();
            uiBlinker.Show(targetTransform);
        }

        public void Blink(Town town)
        {
            uiBlinker.Hide();
            mapBlinker.Show(town.WorldLocation);
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

        private static OnboardingSequence IntroSequence()
        {
            var mapModeTask = new OnboardingTask("Press [F2] to change the map overlay");
            return new OnboardingSequence(
                new OnboardingExplainerStep(0), // 0
                new OnboardingExplainerStep(1),
                new OnboardingTaskStep(mapModeTask),
                new OnboardingMapOverlayStep(mapModeTask),
                new OnboardingTaskClearStep()
            );
        }

        private OnboardingSequence HayDeliverySequence()
        {
            var buyHayTask = new OnboardingTask($"Buy 15 Hay in {_townA.Name}");
            var goToATask = new OnboardingTask($"Travel to {_townB.Name}");
            var sellHayTask = new OnboardingTask($"Sell 15 Hay in {_townB.Name}");

            return new OnboardingSequence(
                new OnboardingExplainerStep(2), //3
                new OnboardingTaskStep(buyHayTask, goToATask, sellHayTask),
                new OnboardingTradeStep(TradeType.Buy, Good.T1Hay, 15, buyHayTask),
                new OnboardingTravelStep(_townB, goToATask),
                new OnboardingTradeStep(TradeType.Sell, Good.T1Hay, 15, sellHayTask),
                new OnboardingTaskClearStep()
            );
        }

        private OnboardingSequence BerryPickerSequence()
        {
            var buildBerryPickerTask = new OnboardingTask($"Build berry picker in {_townB.Name}");

            var buildBerryPickerSequence = new OnboardingSequence(
                new OnboardingExplainerStep(3),
                new SimpleOnboardingStep(() =>
                {
                    _townA.DevelopmentManager.AddDevelopmentChange(100);
                    _player.Inventory.Funds.Value = 505f;
                }),
                new OnboardingExplainerStep(4),
                new OnboardingExplainerStep(5),
                new OnboardingTaskStep(buildBerryPickerTask),
                new OnboardingBuildProducerStep(_townB, Good.T1Berries, buildBerryPickerTask),
                new SimpleOnboardingStep(() =>
                {
                    var berryCount = _townB.Inventory.Get(Good.T1Berries);
                    _townB.Inventory.AddGood(Good.T1Berries, 20 - berryCount);
                }),
                new OnboardingTaskClearStep()
            );
            return buildBerryPickerSequence;
        }

        private static OnboardingSequence GameSpeedControlsSequence()
        {
            var pauseGameTask = new OnboardingTask("Pause the game [Space]");
            var speedUpGameTask = new OnboardingTask("Set the game speed to fast [F2]");

            return new OnboardingSequence(
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

            return new OnboardingSequence(
                new OnboardingExplainerStep(8),
                new OnboardingExplainerStep(9),
                new OnboardingTaskStep(
                    upgradeCartTask,
                    buyBerriesTask,
                    buyGameTask,
                    goToATask,
                    sellBerriesTask,
                    sellGameTask),
                new OnboardingTradeStep(TradeType.Buy, Good.T1Berries, berryCount, buyBerriesTask),
                new OnboardingTradeStep(TradeType.Buy, Good.T1WildGame, gameCount, buyBerriesTask),
                new OnboardingTravelStep(_townA, goToATask),
                new OnboardingTradeStep(TradeType.Sell, Good.T1Berries, berryCount, sellBerriesTask),
                new OnboardingTradeStep(TradeType.Sell, Good.T1WildGame, gameCount, sellBerriesTask),
                new OnboardingTaskClearStep()
            );
        }

        private OnboardingSequence FinishOnboardingSequence()
        {
            var townUpgradeTask = new OnboardingTask($"Upgrade {_townA.Name} to tier II.");

            return new OnboardingSequence(
                new OnboardingExplainerStep(10),
                new OnboardingExplainerStep(11),
                new OnboardingExplainerStep(12),
                new OnboardingTaskStep(townUpgradeTask),
                new OnboardingTownUpgradeStep(_townA, Tier.Tier2, townUpgradeTask),
                new OnboardingTaskClearStep()
            );
        }

        #endregion
    }
}