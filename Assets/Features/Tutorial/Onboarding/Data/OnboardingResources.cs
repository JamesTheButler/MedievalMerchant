using AYellowpaper.SerializedCollections;
using Common.Utility;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Data
{
    
    [CreateAssetMenu(
        fileName = nameof(OnboardingResources),
        menuName = AssetMenu.ResourceFolder + nameof(OnboardingResources))]
    public sealed class OnboardingResources : ScriptableObject
    {
        [SerializeField]
        public SerializedDictionary<int, string> explainerTexts;
    }
}