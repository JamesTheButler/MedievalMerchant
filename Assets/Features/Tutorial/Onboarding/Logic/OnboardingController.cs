using System;
using System.Collections;
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
    public sealed class OnboardingStepSegment : IOnboardingStep
    {
        public OnboardingTask Task => null;

        private IOnboardingStep[] _steps;

        public OnboardingStepSegment(params IOnboardingStep[] steps)
        {
            _steps = steps;
        }

        public IEnumerator Run(OnboardingController controller)
        {
            return _steps.Select(step => step.Run(controller)).GetEnumerator();
        }

        public void Initialize() { }
        public void CleanUp() { }
    }

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

        private List<IOnboardingStep> _steps;

        private OnboardingResources _onboardingResources;
        private PlayerModel _player;
        private Town _townA, _townB;

        private int _explainerIndex;

        public override void Initialize()
        {
            var model = GameplayContext.Instance.Model;
            _player = model.Player;
            _townA = model.Towns.Values.ElementAt(0);
            _townB = model.Towns.Values.ElementAt(1);

            _onboardingResources = ResourceManager.Instance.OnboardingResources;

            _steps = new List<IOnboardingStep>
            {
                HayDeliverySegment(),
                BerryPickerSegment(),
                GameSpeedControlsSegment(),

                new OnboardingExplainerStep(),
                new OnboardingExplainerStep(),
            };
        }

        private OnboardingStepSegment HayDeliverySegment()
        {
            var buyHayTask = new OnboardingTask($"Buy 15 Hay in {_townA.Name}");
            var goToATask = new OnboardingTask($"Travel to {_townB.Name}");
            var sellHayTask = new OnboardingTask($"Sell 15 Hay in {_townB.Name}");

            var deliverHaySegment = new OnboardingStepSegment(
                new OnboardingExplainerStep(),
                new OnboardingExplainerStep(),
                new OnboardingExplainerStep(),
                new OnboardingTaskStep(buyHayTask, goToATask, sellHayTask),
                new OnboardingTradeStep(TradeType.Buy, Good.T1Hay, 15, buyHayTask),
                new OnboardingTravelStep(_townB, goToATask),
                new OnboardingTradeStep(TradeType.Sell, Good.T1Hay, 15, sellHayTask),
                new OnboardingTaskClearStep()
            );
            return deliverHaySegment;
        }

        private OnboardingStepSegment BerryPickerSegment()
        {
            var buildBerryPickerTask = new OnboardingTask($"Build berry picker in {_townB.Name}");

            var buildBerryPickerSegment = new OnboardingStepSegment(
                new OnboardingExplainerStep(),
                new SimpleOnboardingStep(() =>
                {
                    _townA.DevelopmentManager.AddDevelopmentChange(100);
                    _player.Inventory.Funds.Value = 505f;
                }),
                new OnboardingExplainerStep(),
                new OnboardingExplainerStep(),
                new OnboardingTaskStep(buildBerryPickerTask),
                new OnboardingBuildProducerStep(_townB, Good.T1Berries, buildBerryPickerTask),
                new SimpleOnboardingStep(() =>
                {
                    var berryCount = _townB.Inventory.Get(Good.T1Berries);
                    _townB.Inventory.AddGood(Good.T1Berries, 20 - berryCount);
                }),
                new OnboardingTaskClearStep()
            );
            return buildBerryPickerSegment;
        }

        private static OnboardingStepSegment GameSpeedControlsSegment()
        {
            var pauseGameTask = new OnboardingTask("Pause the game [Space]");
            var speedUpGameTask = new OnboardingTask("Set the game speed to fast [F2]");

            return new OnboardingStepSegment(
                new OnboardingExplainerStep(),
                new OnboardingExplainerStep(),
                new OnboardingTaskStep(pauseGameTask, speedUpGameTask),
                new OnboardingResumeGameTask(pauseGameTask),
                new OnboardingSetGameSpeedTask(speedUpGameTask),
                new OnboardingTaskClearStep()
            );
        }

        public void StartTutorial()
        {
            if (_tutorialCoroutine != null)
            {
                StopCoroutine(_tutorialCoroutine);
            }

            _tutorialCoroutine = StartCoroutine(RunTutorial(_steps));
        }

        public void PostExplainer(Action onNextClicked)
        {
            var message = _onboardingResources.explainerTexts
                .GetValueOrDefault(_explainerIndex, "Error")
                .Replace("Town A", _townA.Name, StringComparison.InvariantCultureIgnoreCase)
                .Replace("TownA", _townA.Name, StringComparison.InvariantCultureIgnoreCase)
                .Replace("Town B", _townB.Name, StringComparison.InvariantCultureIgnoreCase)
                .Replace("TownB", _townB.Name, StringComparison.InvariantCultureIgnoreCase);

            explainerUI.Show(message, onNextClicked);
            _explainerIndex++;
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

        private IEnumerator RunTutorial(IEnumerable<IOnboardingStep> steps)
        {
            foreach (var step in steps)
            {
                step.Initialize();
                yield return step.Run(this);
                step.CleanUp();
                step.Task?.Complete();
                yield return new WaitForSeconds(1f);
            }
        }
    }
}