using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Player.Logic;
using Features.Player.Retinue.Config;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionUpgradeService : IService
    {
        private CompanionConfig _companionConfig;
        private PlayerModel _player;


        public void Initialize()
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            _player = GameplayContext.Instance.Model.Player;
        }

        public void CleanUp() { }

        public void LevelUpgradeRequested(CompanionType companionType, int newLevel)
        {
            var companionConfigData = _companionConfig.Get(companionType);
            var levels = companionConfigData.Levels;

            if (newLevel > levels.Count)
            {
                Debug.LogError(
                    $"Upgrade of companion {companionType} failed. New level: {newLevel}, max level : {levels.Count}");
                return;
            }

            var baseCost = companionConfigData.GetLevelData(newLevel).Cost;
            var cost = new ModifiableVariable("Upgrade Cost", false, new CompanionUpgradeBaseCostModifier(baseCost));

            var negotiatorLevel = _player.RetinueManager.CompanionLevels[CompanionType.Negotiator];
            if (negotiatorLevel > 0)
            {
                var levelData = _companionConfig.NegotiatorData.GetTypedLevelData(negotiatorLevel);
                var costReduction = -levelData.UpgradeCostReduction;
                // TODO - STYLE: this should be a proper modifier
                cost.AddModifier(new GenericBasePercentageModifier(costReduction));
            }

            if (!_player.Inventory.HasFunds((int)cost))
            {
                Debug.LogError($"Player does not have enough coin to upgrade ({_player.Inventory.Funds}/{(int)cost})");
                return;
            }

            _player.RetinueManager.SetLevel(companionType, newLevel);
            _player.Inventory.RemoveFunds((int)cost);
        }
    }
}