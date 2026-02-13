using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Player.Retinue.Config;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionMissionSystem : ISystem
    {
        private readonly CompanionType _companionType;

        private CompanionConfig _companionConfig;
        private CompanionModel _companionModel;

        public CompanionMissionSystem(CompanionType companionType)
        {
            _companionType = companionType;
        }

        public void Initialize()
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            _companionModel = GameplayContext.Instance.Model.Player.RetinueModel.Companions[_companionType];

            _companionModel.Level.Observe(OnLevelChanged);
        }

        public void CleanUp()
        {
            _companionModel.Level.StopObserving(OnLevelChanged);
        }

        private void OnLevelChanged(int level)
        {
            var missionConfig = _companionConfig.Get(_companionModel.CompanionType).MissionConfig;

            var nextMissionConfig = missionConfig.ConfigsPerLevel.ElementAtOrDefault(level);
            if (nextMissionConfig == null)
            {
                _companionModel.ActiveMission.Value = null;
                return;
            }

            var missionTargets = new Dictionary<Good, int>();

            foreach (var item in nextMissionConfig.Items)
            {
                missionTargets.Add(item.Good, item.Amount);
            }

            _companionModel.StartMission(nextMissionConfig.Cost, missionTargets);
        }
    }
}