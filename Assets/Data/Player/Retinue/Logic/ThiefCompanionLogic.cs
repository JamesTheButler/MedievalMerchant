using Data.Player.Retinue.Config.CompanionDatas;
using Data.Player.Retinue.Config.LevelDatas;
using Data.Towns;
using UnityEngine;

namespace Data.Player.Retinue.Logic
{
    public sealed class ThiefCompanionLogic : BaseCompanionLogic<ThiefCompanionData>
    {
        private PlayerModel _player;

        protected override CompanionType Type => CompanionType.Thief;

        private ThiefLevelData _thiefLevelData;

        private bool _isBound;

        // TODO: this seems quite generic
        //   the companion data should have base implementation to get current level data 
        public override void SetLevel(int level)
        {
            _thiefLevelData = ConfigData.GetLevelData(level);

            if (_thiefLevelData is null)
            {
                Debug.LogError($"Could not find level data for {Type}, {level}");
                return;
            }

            if (_isBound) return;
            Bind();
        }

        private void Bind()
        {
            _player = Model.Instance.Player;
            _player.Location.TownEntered += OnTownChanged;

            _isBound = true;
        }

        // TODO: exploit possible: reentering town frequently to get coins fast
        private void OnTownChanged(Town enteredTown)
        {
            if (enteredTown == null) return;

            _player.Inventory.AddFunds((int)_thiefLevelData.TownEntranceGold);
            enteredTown.Inventory.RemoveFunds((int)_thiefLevelData.TownEntranceGold);

            var isThiefCaught = RandomUtility.GetBool(_thiefLevelData.ReputationLossChance);
            if (isThiefCaught)
            {
                enteredTown.RemoveReputation(_thiefLevelData.ReputationLoss);
            }
        }
    }
}