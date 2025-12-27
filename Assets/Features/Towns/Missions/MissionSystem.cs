using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Common.Utility;
using Features.Goods;
using Features.Goods.Config;
using Features.Ticking;
using Features.Towns.Missions.Data;
using Features.Towns.Missions.Results;
using Features.Trade;

namespace Features.Towns.Missions
{
    public sealed class MissionSystem : ISystem
    {
        private readonly Town _town;
        private readonly MissionModel _missionModel;
        private readonly MissionResultHandler _resultHandler;

        private TickingService _tickingService;
        private Date _gameDate;

        private MissionConfig _missionConfig;
        private TradeMissionConfigData _tradeConfig;
        private GoodsResources _goodsResources;
        private GoodPool _goodPool;

        private HashSet<Good> _availableGoods;

        public MissionSystem(Town town)
        {
            _town = town;
            _missionModel = town.Missions;
            _resultHandler = new MissionResultHandler(town);
        }

        public void Initialize()
        {
            _tickingService = GameplayContext.Instance.Services.TickingService;
            _missionConfig = ConfigurationManager.Configurations.MissionConfig;
            _tradeConfig = _missionConfig.TradeMissionData;
            _gameDate = GameplayContext.Instance.Model.Date;
            _goodPool = GameplayContext.Instance.Model.GoodPool;
            _goodsResources = ResourceManager.Instance.GoodsResources;

            _tickingService.DayPassed += OnDayPassed;
            _town.TradeCompleted += OnTradeCompleted;
            _town.DevelopmentManager.Tier.Observe(ResetAvailableGoods);
        }

        private void ResetAvailableGoods(Tier tier)
        {
            // start with all available goods on the map
            _availableGoods = _goodPool.GetAvailableGoods().ToHashSet();
            // remove from wrong tiers
            var goodsOfWrongTier = _goodsResources.ConfigData
                .Where(kvPair => kvPair.Value.Tier != tier)
                .Select(kvPair => kvPair.Key);
            _availableGoods.RemoveFrom(goodsOfWrongTier);
            // remove all goods from local regions
            _availableGoods.RemoveFrom(_town.AvailableGoods);
            // remove all goods from ongoing missions in this town
            _availableGoods.RemoveFrom(_missionModel.Missions.Keys);
        }

        public void CleanUp()
        {
            _tickingService.DayPassed -= OnDayPassed;
            _town.TradeCompleted -= OnTradeCompleted;
        }

        private void OnTradeCompleted(TradeInfo tradeInfo)
        {
            // only progress missions when selling to town
            if (tradeInfo.Type == TradeType.Buy)
                return;

            if (!_missionModel.Missions.TryGetValue(tradeInfo.Good, out var mission))
                return;

            mission.Deliver(tradeInfo.Amount);
            if (mission.IsSucceeded)
            {
                UntrackMission(mission);
            }
        }

        private void OnDayPassed()
        {
            TryFailOngoingMissions();
            TryTriggerNewMission();
        }

        private void TryFailOngoingMissions()
        {
            foreach (var mission in _missionModel.Missions.Values)
            {
                if (_gameDate <= mission.EndDate)
                {
                    mission.Fail();
                    UntrackMission(mission);
                }
            }
        }

        private void TryTriggerNewMission()
        {
            if (_tradeConfig.MaxMissionCount >= _missionModel.Missions.Count)
                return;
            
            var isMissionTriggered = RandomUtility.GetBool(_missionConfig.TradeMissionData.DailyMissionChance);
            if (!isMissionTriggered)
                return;

            var missionGood = _availableGoods.GetRandom();
            _tradeConfig = _missionConfig.TradeMissionData;
            var mission = new Mission(
                missionGood,
                _tradeConfig.Volume,
                _gameDate + _tradeConfig.LengthInDays,
                _tradeConfig.GetReward(),
                _tradeConfig.GetPenalty());

            TrackMission(mission);
        }

        private void TrackMission(Mission mission)
        {
            _missionModel.AddMission(mission);
            // remove from available goods to ensure no duplicate missions
            _availableGoods.Remove(mission.Good);

            mission.MissionFailed += OnMissionFailed;
            mission.MissionSucceeded += OnMissionSucceeded;
        }

        private void OnMissionSucceeded(IMissionResult result)
        {
            _resultHandler.Handle(result);
        }

        private void OnMissionFailed(IMissionResult result)
        {
            _resultHandler.Handle(result);
        }

        private void UntrackMission(Mission mission)
        {
            mission.MissionFailed -= OnMissionFailed;
            mission.MissionSucceeded -= OnMissionSucceeded;

            // add good back into available pool (if of correct tier)
            var goodTier = _goodsResources.ConfigData[mission.Good].Tier;
            if (goodTier == _town.Tier.Value)
            {
                _availableGoods.Add(mission.Good);
            }

            _missionModel.RemoveMission(mission);
        }
    }
}