using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements;
using Features.Player.Caravan.UI;
using Features.Towns;
using Features.Towns.Production.UI;
using Features.Towns.UI;
using Features.Trade;
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

        [SerializeField]
        private SerializedDictionary<int, string> explainerTexts;

        private Coroutine _tutorialCoroutine;

        private List<IOnboardingStep> _steps;

        private Town _townA, _townB;

        public override void Initialize()
        {
            _townA = GameplayContext.Instance.Model.Towns.Values.ElementAt(0);
            _townB = GameplayContext.Instance.Model.Towns.Values.ElementAt(1);

            _steps = new List<IOnboardingStep>
            {
                new OnboardingExplainerStep(0),
                new OnboardingExplainerStep(1),
                new OnboardingExplainerStep(2),
                new OnboardingTaskStep(new List<string>
                {
                    $"Buy 15 Hay in {_townA.Name}",
                    $"Travel to {_townB.Name}",
                    $"Sell 15 Hay in {_townB.Name}",
                }),
                new OnboardingTradeStep(TradeType.Buy, Good.T1Hay, 15),
                new OnboardingTravelStep(_townB),
                new OnboardingTradeStep(TradeType.Sell, Good.T1Hay, 15),
                new OnboardingTaskClearStep(),
            };
        }

        public void StartTutorial()
        {
            if (_tutorialCoroutine != null)
            {
                StopCoroutine(_tutorialCoroutine);
            }

            _tutorialCoroutine = StartCoroutine(RunTutorial(_steps));
        }

        public void PostExplainer(int index, Action onNextClicked)
        {
            var message = explainerTexts
                .GetValueOrDefault(index, "Error")
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

        public void AddTasks(List<string> tasks)
        {
            taskListUI.SetUp(tasks.Select(taskMsg => new OnboardingTask(taskMsg)));
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
                yield return new WaitForSeconds(1f);
            }
        }
    }
}