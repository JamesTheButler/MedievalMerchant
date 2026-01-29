using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Common.UI.Elements;
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

        [SerializeField]
        private SerializedDictionary<int, string> explainerTexts;

        private Coroutine _tutorialCoroutine;
        private OnboardingContext _context;

        private List<IOnboardingStep> _steps;

        public override void Initialize()
        {
            _context = new OnboardingContext(explainerUI, uiBlinker, mapBlinker, explainerTexts);

            _steps = new List<IOnboardingStep>
            {
                new OnboardingExplainerStep(0),
                new OnboardingExplainerStep(1),
                new OnboardingExplainerStep(2),
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

        private IEnumerator RunTutorial(IEnumerable<IOnboardingStep> steps)
        {
            foreach (var step in steps)
            {
                step.Initialize();
                yield return step.Run(_context);
                step.CleanUp();
            }
        }
    }
}