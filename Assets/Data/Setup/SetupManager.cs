using UnityEngine;

namespace Data.Setup
{
    public sealed class SetupManager : MonoBehaviour
    {
        public static SetupManager Instance;

        [field: SerializeField]
        public GoodInfoManager GoodInfoManager { get; private set; }

        [field: SerializeField]
        public DevelopmentSetup DevelopmentSetup { get; private set; }

        [field: SerializeField]
        public MarketStateMultipliers MarketStateMultipliers { get; private set; }

        [field: SerializeField]
        public MarketStateIcons MarketStateIcons { get; private set; }

        [field: SerializeField]
        public MarketStateThresholds MarketStateThresholds { get; private set; }

        [field: SerializeField]
        public GrowthTrendIcons GrowthTrendIcons { get; private set; }

        [field: SerializeField]
        public Colors Colors { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}