using AYellowpaper.SerializedCollections;
using Common.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Tutorial.Onboarding.Data
{
    
    [CreateAssetMenu(
        fileName = nameof(OnboardingResources),
        menuName = AssetMenu.ResourceFolder + nameof(OnboardingResources))]
    public sealed class OnboardingResources : ScriptableObject
    {
        [SerializeField]
        public SerializedDictionary<OnboardingExplainer, LocalizedString> explainerTexts;
    }
}