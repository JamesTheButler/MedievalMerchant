using Data.Configuration;
using Data.Player.Retinue.Config;
using Data.Player.Retinue.UI;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Player.Retinue
{
    public sealed class CompanionUpgrader : MonoBehaviour
    {
        [SerializeField, Required] private Transform companionGroup;

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
            var levels = companionConfigData.TypedLevels;

            if (newLevel > levels.Count)
            {
                Debug.LogError(
                    $"Upgrade of companion {companionType} failed. New level: {newLevel}, max level : {levels.Count}");
                return;
            }

            var cost = companionConfigData.TypedLevels[newLevel - 1].Cost;

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