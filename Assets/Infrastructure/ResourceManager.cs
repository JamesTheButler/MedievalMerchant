using Common.Config;
using Features.Goods.Config;
using Features.Towns.Config;
using Features.Towns.Development.UI.DevelopmentGauge;
using Features.Towns.Flags.Config;
using NaughtyAttributes;
using UnityEngine;

namespace Infrastructure
{
    [ExecuteInEditMode]
    public sealed class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance;

        [field: SerializeField, Required]
        public AvailabilityResources AvailabilityResources { get; private set; }

        [field: SerializeField, Required]
        public CaravanResources CaravanResources { get; private set; }

        [field: SerializeField, Required]
        public Colors Colors { get; private set; }

        [field: SerializeField, Required]
        public Cursors Cursors { get; private set; }

        [field: SerializeField, Required]
        public DevelopmentMilestoneResources DevelopmentMilestoneResources { get; private set; }

        [field: SerializeField, Required]
        public FlagResources FlagResources { get; private set; }

        [field: SerializeField, Required]
        public RecipeResources RecipeResources { get; private set; }

        [field: SerializeField, Required]
        public RegionResources RegionResources { get; private set; }

        [field: SerializeField, Required]
        public TierResources TierResources { get; private set; }

        [field: SerializeField, Required]
        public TownResources TownResources { get; private set; }

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

            RecipeResources.Initialize();
        }
    }
}