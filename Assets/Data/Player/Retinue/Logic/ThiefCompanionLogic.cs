using Data.Player.Retinue.Config;
using Data.Towns;
using Selection = Data.Towns.Selection;

namespace Data.Player.Retinue.Logic
{
    public sealed class ThiefCompanionLogic : BaseCompanionLogic<ThiefLevelData>
    {
        private readonly PlayerModel _player = Model.Instance.Player;
        private readonly Selection _selection = Selection.Instance;

        protected override CompanionType Type => CompanionType.Guard;

        private bool isBound;

        // TODO: this seems quite generic
        //   the companion data should have base implementation to get current level data 
        protected override void OnLevelChanged(int level)
        {
            if (isBound) return;
            Bind();
        }

        private void Bind()
        {
            _selection.TownSelected += OnTownChanged;

            isBound = true;
        }

        // TODO: exploit possible: reentering town frequently to get coins fast
        private void OnTownChanged(Town enteredTown)
        {
            if (enteredTown == null) return;

            var stolenCoin = (int)LevelData.TownEntranceGold;
            _player.Inventory.AddFunds(stolenCoin);
            enteredTown.Inventory.RemoveFunds(stolenCoin);

            var isThiefCaught = RandomUtility.GetBool(LevelData.ReputationLossChance);
            if (isThiefCaught)
            {
                _selection.SelectedTown.Reputation.Value -= LevelData.ReputationLoss;
            }
        }
    }
}