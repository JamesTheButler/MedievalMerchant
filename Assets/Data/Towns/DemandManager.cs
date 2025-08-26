using Data.Setup;

namespace Data.Towns
{
    public sealed class DemandManager
    {
        private readonly Town _town;
        private readonly DemandMultipliers _demandMultipliers;

        public DemandManager(Town town)
        {
            _town = town;
            _demandMultipliers = SetupManager.Instance.DemandMultipliers;
        }

        public float GetDemandMultiplier()
        {
            
        }
    }
}