using Common.Infrastructure;

namespace Features.Player.Retinue.Logic
{
    /// <summary>
    /// Companion upgrades are now handled automatically via mission completion
    /// in <see cref="CompanionMissionSystem"/>. Goods and coin are delivered
    /// through <see cref="CompanionDeliveryService"/> from the camp UI.
    /// </summary>
    public sealed class CompanionUpgradeService : IService
    {
        public void Initialize() { }
        public void CleanUp() { }
    }
}
