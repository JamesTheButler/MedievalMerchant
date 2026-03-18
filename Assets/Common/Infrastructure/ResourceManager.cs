using Common.Config;
using Common.UI.Elements.Animation;
using Features.Audio.Data;
using Features.Goods.Config;
using Features.Goods.Recipe.Data;
using Features.Localization.Data;
using Features.Player.Retinue.Config;
using Features.Player.Retinue.Config.Resources;
using Features.Towns.Config;
using Features.Towns.Development.UI.DevelopmentGauge;
using Features.Towns.Flags.Config;
using Features.Towns.Production.Config;
using Features.Towns.Reputation.Data;
using Features.Trade.Haggling.Data;
using Features.Tutorial.Data;
using Features.Tutorial.Onboarding.Data;
using NaughtyAttributes;
using UnityEngine;
using ConditionResources = Features.Levels.Conditions.Config.ConditionResources;

namespace Common.Infrastructure
{
    [ExecuteInEditMode]
    public sealed class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance;

        [field: SerializeField, Required]
        public AnimationResources AnimationResources { get; private set; }

        [field: SerializeField, Required]
        public AudioResources AudioResources { get; private set; }

        [field: SerializeField, Required]
        public AvailabilityResources AvailabilityResources { get; private set; }

        [field: SerializeField, Required]
        public CaravanResources CaravanResources { get; private set; }

        [field: SerializeField, Required]
        public CompanionResources CompanionResources { get; private set; }

        [field: SerializeField, Required]
        public ConditionResources ConditionResources { get; private set; }

        [field: SerializeField, Required]
        public Cursors Cursors { get; private set; }

        [field: SerializeField, Required]
        public DevelopmentMilestoneResources DevelopmentMilestoneResources { get; private set; }

        [field: SerializeField, Required]
        public FlagResources FlagResources { get; private set; }

        [field: SerializeField, Required]
        public HaggleResources HaggleResources { get; private set; }

        [field: SerializeField, Required]
        public GoodResources GoodResources { get; private set; }

        [field: SerializeField, Required]
        public LocalizationResources LocalizationResources { get; private set; }

        [field: SerializeField, Required]
        public OnboardingResources OnboardingResources { get; private set; }

        [field: SerializeField, Required]
        public RecipeResources RecipeResources { get; private set; }

        [field: SerializeField, Required]
        public RegionResources RegionResources { get; private set; }

        [field: SerializeField, Required]
        public ReputationResources ReputationResources { get; private set; }

        [field: SerializeField, Required]
        public TierResources TierResources { get; private set; }

        [field: SerializeField, Required]
        public TownResources TownResources { get; private set; }

        [field: SerializeField, Required]
        public TutorialResources TutorialResources { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }

                return;
            }

            Instance = this;

            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }

            RecipeResources.Initialize();
        }
    }
}