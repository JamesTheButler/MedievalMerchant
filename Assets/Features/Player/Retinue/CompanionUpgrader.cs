using Common;
using Common.Modifiable;
using Features.Player.Retinue.Config;
using Features.Player.Retinue.UI;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Player.Retinue
{
    public sealed class CompanionUpgrader : MonoBehaviour
    {
        [SerializeField, Required]
        private Transform companionGroup;

        private CompanionConfig _companionConfig;
        private PlayerModel _player;

        private void Start()
        {
            _companionConfig = ConfigurationManager.Instance.CompanionConfig;
            _player = Model.Instance.Player;

            foreach (var companionUi in companionGroup.GetComponentsInChildren<CompanionUI>())
            {
                companionUi.levelUpgradeRequested.AddListener(LevelUpgradeRequested);
            }
        }

        private void LevelUpgradeRequested(CompanionType companionType, int newLevel)
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
            var cost = new ModifiableVariable("Upgrade Cost", new CompanionUpgradeBaseCostModifier(baseCost));

            var negotiatorLevel = _player.RetinueManager.CompanionLevels[CompanionType.Negotiator];
            if (negotiatorLevel > 0)
            {
                var levelData = _companionConfig.NegotiatorData.GetTypedLevelData(negotiatorLevel);
                var costReduction = -levelData.UpgradeCostReduction;
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