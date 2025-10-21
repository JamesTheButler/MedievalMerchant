using Common;
using Features.Player.Retinue.Config.CompanionDatas;

namespace Features.Player.Retinue.Logic
{
    public sealed class NegotiatorCompanionLogic : BaseCompanionLogic<NegotiatorCompanionData>
    {
        protected override CompanionType Type => CompanionType.Negotiator;

        private NegotiatorUpgradeCostModifier _negotiatorCostModifier;

        public override void SetLevel(int level)
        {
            if (level <= 0) return;

            if (_negotiatorCostModifier == null)
            {
                _negotiatorCostModifier = new NegotiatorUpgradeCostModifier(level);
                var caravanManager = Model.Instance.Player.CaravanManager;
                foreach (var cart in caravanManager.Carts)
                {
                    cart.UpgradeCost.AddModifier(_negotiatorCostModifier);
                }

                return;
            }

            _negotiatorCostModifier.Update(level);
        }
    }
}