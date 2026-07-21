using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.Utility;
using Features.Map.Pathfinding;
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
        private DateModel _gameDateModel;
        private Date _nextPossibleTheftDate = new();

        protected override CompanionType Type => CompanionType.Thief;

        private ThiefLevelData _thiefLevelData;

        private bool _isBound;

        public override void SetLevel(int level)
        {
            if (level <= 0) return;

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
            _gameDateModel = GameplayContext.Instance.Model.DateModel;

            _player.Location.MapLocation.Observe(OnLocationChanged, false);

            _isBound = true;
        }

        private void OnLocationChanged(IMapLocation location)
        {
            if (location is not Town town || _thiefLevelData == null) return;

            if (_gameDateModel.GameDate.Value < _nextPossibleTheftDate)
                return;

            _nextPossibleTheftDate = _gameDateModel.GameDate.Value + ConfigData.MinDaysBetweenThefts;


            _player.Inventory.AddFunds(_thiefLevelData.TownEntranceGold);
            town.Inventory.RemoveFunds(_thiefLevelData.TownEntranceGold);

            var log = $"Thief stole {_thiefLevelData.TownEntranceGold} from {town.Name}.";

            var isThiefCaught = RandomUtility.GetBool(_thiefLevelData.ReputationLossChance);
            if (isThiefCaught)
            {
                town.ReputationModel.UpdateReputation(
                    _thiefLevelData.ReputationLoss,
                    "Your thief was caught stealing!");
                log += $".. and got caught ({_thiefLevelData.ReputationLoss} rep)";
            }

            Debug.Log(log);
        }
    }
}