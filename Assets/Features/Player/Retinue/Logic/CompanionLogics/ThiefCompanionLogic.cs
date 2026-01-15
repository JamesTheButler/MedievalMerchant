using Common.Infrastructure.Gameplay;
using Common.Utility;
using Features.Player.Logic;
using Features.Player.Retinue.Config.CompanionDatas;
using Features.Player.Retinue.Config.LevelDatas;
using Features.Towns;
using UnityEngine;

namespace Features.Player.Retinue.Logic.CompanionLogics
{
    public sealed class ThiefCompanionLogic : BaseCompanionLogic<ThiefCompanionData>
    {
        private PlayerModel _player;

        protected override CompanionType Type => CompanionType.Thief;

        private ThiefLevelData _thiefLevelData;

        private bool _isBound;

        public override void SetLevel(int level)
        {
            _thiefLevelData = ConfigData.GetTypedLevelData(level);

            if (_thiefLevelData is null)
            {
                Debug.LogWarning($"Could not find level data for {Type}, {level}");
                return;
            }

            if (_isBound) return;
            Bind();
        }

        private void Bind()
        {
            _player = GameplayContext.Instance.Model.Player;
            _player.Location.CurrentTown.Observe(OnTownChanged);

            _isBound = true;
        }

        private void OnTownChanged(Town town)
        {
            if (town == null || _thiefLevelData == null) return;

            _player.Inventory.AddFunds(_thiefLevelData.TownEntranceGold);
            town.Inventory.RemoveFunds(_thiefLevelData.TownEntranceGold);

            var isThiefCaught = RandomUtility.GetBool(_thiefLevelData.ReputationLossChance);
            if (isThiefCaught)
            {
                town.ReputationManager.ApplyCaughtThief(_thiefLevelData.ReputationLoss);
            }
        }
    }
}