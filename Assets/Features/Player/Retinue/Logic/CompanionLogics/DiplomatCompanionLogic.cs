using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Player.Retinue.Config.CompanionDatas;
using Features.Player.Retinue.Config.LevelDatas;
using Features.Player.Retinue.Logic.Modifiers;
using Features.Towns;

namespace Features.Player.Retinue.Logic.CompanionLogics
{
    public sealed class DiplomatCompanionLogic : BaseCompanionLogic<DiplomatCompanionData>
    {
        protected override CompanionType Type => CompanionType.Diplomat;
        private readonly DiplomatReputationModifier _modifier;
        private readonly DateModel _gameDateModel;

        private DiplomatLevelData _diplomatLevelData;
        private Date _nextPossibleRepGainDate = new();
        
        public DiplomatCompanionLogic()
        {
            _modifier = new DiplomatReputationModifier(0);
            var gameModel = GameplayContext.Instance.Model;

            var player = GameplayContext.Instance.Model.Player;
            player.Location.CurrentTown.Observe(OnTownChanged);
            
            _gameDateModel = GameplayContext.Instance.Model.DateModel;

            foreach (var town in gameModel.Towns.Values)
            {
                town.ReputationManager.AddModifier(_modifier);
            }
        }

        public override void SetLevel(int level)
        {
            if (level <= 0)
                return;

            _diplomatLevelData = ConfigData.GetTypedLevelData(level);
            _modifier.Update(level);
        }
        
        private void OnTownChanged(Town town)
        {
            if (town == null || _diplomatLevelData == null) return;

            if (_gameDateModel.GameDate.Value < _nextPossibleRepGainDate)
                return;

            _nextPossibleRepGainDate = _gameDateModel.GameDate.Value + ConfigData.MinDaysBetweenRepGains;

            town.ReputationManager.UpdateReputation(
                _diplomatLevelData.TownEntranceReputation, 
                "Your diplomat rubbed shoulders with the locals.");
        }
    }
}