using Data.Player.Retinue.Config;
using Data.Towns;
using UnityEngine;
using Selection = Data.Towns.Selection;

namespace Data.Player.Retinue.Logic
{
    public sealed class ThiefCompanionLogic : BaseCompanionLogic<ThiefCompanionData>
    {
        private readonly PlayerModel _player = Model.Instance.Player;
        private readonly Selection _selection = Selection.Instance;

        protected override CompanionType Type => CompanionType.Thief;

        private ThiefLevelData _thiefLevelData;

        private bool _isBound;

        // TODO: this seems quite generic
        //   the companion data should have base implementation to get current level data 
        public override void SetLevel(int level)
        {
            if (level > ConfigData.TypedLevels.Count)
            {
                Debug.LogError($"could not find level data for {Type}, {level}");
                return;
            }

            _thiefLevelData = ConfigData.TypedLevels[level];

            if (_isBound) return;
            Bind();
        }

        private void Bind()
        {
            _selection.TownSelected += OnTownChanged;

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
                enteredTown.Reputation.Value -= _thiefLevelData.ReputationLoss;
            }
        }
    }
}