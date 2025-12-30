using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Common.Utility;
using Features.Goods;
using Features.Goods.Config;
using Features.Goods.Selector;
using Features.Notifications.Logic;
using Features.Ticking.Logic;
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
        private NotificationService _notificationService;
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
            _notificationService = GameplayContext.Instance.Services.NotificationService;
            _missionConfig = ConfigurationManager.Configurations.MissionConfig;
            _tradeConfig = _missionConfig.TradeMissionData;
            _gameDate = GameplayContext.Instance.Model.Date;
            _goodPool = GameplayContext.Instance.Model.GoodPool;
            _goodsResources = ResourceManager.Instance.GoodsResources;

            _tickingService.DayPassed += OnDayPassed;
            _town.TradeCompleted += OnTradeCompleted;
            _town.DevelopmentManager.Tier.Observe(OnTownTierChanged, false);
            _town.Missions.GoodSelectorChanged += OnGSChanged;

            ResetAvailableGoods();
        }

        private void OnGSChanged()
        {
            ResetAvailableGoods();
        }

        public void CleanUp()
        {
            _tickingService.DayPassed -= OnDayPassed;
            _town.TradeCompleted -= OnTradeCompleted;
            _town.DevelopmentManager.Tier.StopObserving(OnTownTierChanged);
            _town.Missions.GoodSelectorChanged -= OnGSChanged;
        }

        private void OnTownTierChanged(Tier tier)
        {
            ResetAvailableGoods();
        }

        private void ResetAvailableGoods()
        {
            // start with all available goods on the map
            _availableGoods = _goodPool.AllAvailableGoods.ToHashSet();
            // remove from wrong tiers
            var goodsOfWrongTier = _goodsResources.ResourceData
                .Where(kvPair => kvPair.Value.Tier != _town.Tier.Value)
                .Select(kvPair => kvPair.Key);
            _availableGoods.RemoveFrom(goodsOfWrongTier);
            // remove all goods from local regions
            _availableGoods.RemoveFrom(_town.AvailableGoods);
            // remove all goods from ongoing missions in this town
            _availableGoods.RemoveFrom(_missionModel.Missions.Keys);
            // remove goods disallowed by MissionModel.Selector
            if (_missionModel.PermittedGoodsSelector != IGoodSelector.All)
            {
                _availableGoods.RemoveWhere(good => !_missionModel.PermittedGoodsSelector.Matches(good));
            }
        }

        private void OnTradeCompleted(TradeInfo tradeInfo)
        {
            // only progress missions when selling to town
            if (tradeInfo.Type == TradeType.Buy)
                return;

            if (!_missionModel.Missions.TryGetValue(tradeInfo.Good, out var mission))
                return;

            mission.Deliver(tradeInfo.Amount);
        }

        private void OnDayPassed()
        {
            ValidateOngoingMissions();
            TryTriggerNewMission();
        }

        private void ValidateOngoingMissions()
        {
            foreach (var mission in _missionModel.Missions.Values.ToArray())
            {
                mission.ValidateDate(_gameDate);
            }
        }

        private void TryTriggerNewMission()
        {
            if (_missionModel.Missions.Count >= _tradeConfig.MaxMissionCount)
                return;

            var isMissionTriggered = RandomUtility.GetBool(_missionConfig.TradeMissionData.DailyMissionChance);
            if (!isMissionTriggered)
                return;

            if (_availableGoods.IsEmpty())
                return;

            var missionGood = _availableGoods.GetRandom();
            var mission = new Mission(
                missionGood,
                _tradeConfig.Volume,
                _gameDate + _tradeConfig.LengthInDays,
                MissionType.TradeMission,
                _tradeConfig.GetReward(),
                _tradeConfig.GetPenalty());

            mission.ValidateDate(_gameDate);

            EnableMission(mission);
        }

        private void EnableMission(Mission mission)
        {
            _missionModel.AddMission(mission);

            mission.MissionFailed += OnMissionFailed;
            mission.MissionSucceeded += OnMissionSucceeded;

            // remove from available goods to ensure no duplicate missions
            _availableGoods.Remove(mission.Good);

            var notification = new MissionStartedNotification(_town, mission);
            _notificationService.PostNotification(notification);
        }

        private void OnMissionSucceeded(Mission mission)
        {
            _resultHandler.Handle(mission.Reward);

            DisableMission(mission);
        }

        private void OnMissionFailed(Mission mission)
        {
            var notification = new MissionFailedNotification(_town, mission);
            _notificationService.PostNotification(notification);
            _resultHandler.Handle(mission.Penalty);

            DisableMission(mission);
        }

        private void DisableMission(Mission mission)
        {
            mission.MissionFailed -= OnMissionFailed;
            mission.MissionSucceeded -= OnMissionSucceeded;

            // add good back into available pool (if of correct tier)
            var goodTier = _goodsResources.ResourceData[mission.Good].Tier;
            if (goodTier == _town.Tier.Value)
            {
                _availableGoods.Add(mission.Good);
            }

            _missionModel.RemoveMission(mission);
        }
    }
}