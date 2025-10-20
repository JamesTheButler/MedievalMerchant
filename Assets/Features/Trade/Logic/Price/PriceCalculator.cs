using System.Linq;
using Common;
using Features.Towns.Development.Logic.Milestones;
using Common.Modifiable;
using Common.Types;
using Features.Goods.Config;
using Features.Player;
using Features.Player.Retinue;
using Features.Player.Retinue.Logic;
using Features.Towns;

namespace Features.Trade.Logic.Price
{
    public sealed class PriceCalculator
    {
        public ModifiableVariable Price { get; private set; }

        private readonly PlayerModel _player;
        private readonly Town _town;
        private readonly AvailabilityCalculator _availabilityCalculator;
        private readonly GoodsConfig _goodsConfig;

        private AvailabilityPriceModifier _availabilityModifier;
        private NegotiatorPriceModifier _negotiatorModifier;

        private Good _good;
        private TradeType _tradeType;

        public PriceCalculator(Town town)
        {
            _player = Model.Instance.Player;
            _town = town;
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            _availabilityCalculator = new AvailabilityCalculator(town);
        }

        public void Initialize(Good good, TradeType tradeType)
        {
            _good = good;
            _tradeType = tradeType;

            var goodTier = _goodsConfig.ConfigData[_good].Tier;
            var goodBasePrice = _goodsConfig.BasePriceData[goodTier];

            var basePriceModifier = new BasePriceModifier(goodBasePrice, goodTier);
            Price = new ModifiableVariable("Price per Good", basePriceModifier);

            AddAvailabilityModifier();
            AddRegionModifiers();
            AddDevelopmentMilestoneModifiers();
            AddNegotiatorModifier();

            _player.RetinueManager.CompanionLevels[CompanionType.Negotiator].Observe(OnCompanionChanged);
            _town.Inventory.GoodUpdated += OnTownInventoryChanged;
            _town.UpgradeManager.MilestoneModifierAdded += TownModifierAdded;
            _town.UpgradeManager.MilestoneModifierRemoved += TownModifierRemoved;
        }

        public void Clear()
        {
            _player.RetinueManager.CompanionLevels[CompanionType.Negotiator].StopObserving(OnCompanionChanged);
            _town.Inventory.GoodUpdated -= OnTownInventoryChanged;
            _town.UpgradeManager.MilestoneModifierAdded -= TownModifierAdded;
            _town.UpgradeManager.MilestoneModifierRemoved -= TownModifierRemoved;
        }

        #region Adding Modifiers

        private void AddAvailabilityModifier()
        {
            var availability = _availabilityCalculator.GetAvailability(_good);
            _availabilityModifier = new AvailabilityPriceModifier(availability);
            Price.AddModifier(_availabilityModifier);
        }

        private void AddDevelopmentMilestoneModifiers()
        {
            var upgradeModifiers = _town.UpgradeManager.MilestoneModifiers.OfType<MilestonePriceBoostModifier>();
            foreach (var upgradeModifier in upgradeModifiers)
            {
                Price.AddModifier(upgradeModifier);
            }
        }

        private void AddRegionModifiers()
        {
            // don't apply region modifier when buying from town
            if (_tradeType != TradeType.Sell)
                return;

            var goodRegions = _goodsConfig.ConfigData[_good].Regions;
            var isLocal = _town.Regions.Intersects(goodRegions);

            IModifier regionModifier = isLocal
                ? new LocalGoodPriceModifier()
                : new ForeignGoodPriceModifier();
            Price.AddModifier(regionModifier);
        }

        private void AddNegotiatorModifier()
        {
            var negotiatorLevel = _player.RetinueManager.CompanionLevels[CompanionType.Negotiator];
            if (negotiatorLevel == 0)
                return;

            _negotiatorModifier = new NegotiatorPriceModifier(negotiatorLevel, _tradeType);

            Price.AddModifier(_negotiatorModifier);
        }

        #endregion

        #region model change listeners

        private void OnCompanionChanged(int newLevel)
        {
            if (newLevel <= 0)
                return;

            if (_negotiatorModifier == null)
            {
                _negotiatorModifier = new NegotiatorPriceModifier(newLevel, _tradeType);
                Price.AddModifier(_negotiatorModifier);
            }

            _negotiatorModifier.Update(newLevel);
        }

        private void OnTownInventoryChanged(Good good, int amount)
        {
            if (good != _good)
                return;

            var availability = _availabilityCalculator.GetAvailability(good);
            _availabilityModifier.Update(availability);
        }

        private void TownModifierAdded(IModifier modifier)
        {
            if (modifier is MilestonePriceBoostModifier)
            {
                Price.AddModifier(modifier);
            }
        }

        private void TownModifierRemoved(IModifier modifier)
        {
            if (modifier is MilestonePriceBoostModifier)
            {
                Price.RemoveModifier(modifier);
            }
        } 

        #endregion
    }
}