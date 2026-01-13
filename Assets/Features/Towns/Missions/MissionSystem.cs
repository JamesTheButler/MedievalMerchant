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
using Features.Towns.Development.Logic;
using Features.Towns.Missions.Data;
using Features.Towns.Missions.Results;
using Features.Trade;
using UnityEngine;

namespace Features.Towns.Missions
{
    public sealed class MissionSystem : ISystem
    {
        private readonly Town _town;
        private readonly DevelopmentManager _developmentManager;
        private readonly MissionModel _missionModel;
        private readonly MissionResultHandler _resultHandler;

        private TickingService _tickingService;
        private NotificationService _notificationService;
        private Date _gameDate;

        private MissionConfig _missionConfig;
        private TradeMissionConfigData _tradeMissionConfig;
        private UpgradeMissionConfigData _upgradeMissionConfig;
        private GoodResources _goodResources;
        private GoodPool _goodPool;

        private HashSet<Good> _availableGoods;

        public MissionSystem(Town town)
        {
            _town = town;
            _missionModel = town.Missions;
            _developmentManager = town.DevelopmentManager;
            _resultHandler = new MissionResultHandler(town);
        }

        public void Initialize()
        {
            _tickingService = GameplayContext.Instance.Services.TickingService;
            _notificationService = GameplayContext.Instance.Services.NotificationService;
            _missionConfig = ConfigurationManager.Configurations.MissionConfig;
            _tradeMissionConfig = _missionConfig.TradeMissionData;
            _upgradeMissionConfig = _missionConfig.UpgradeMissionData;
            _gameDate = GameplayContext.Instance.Model.Date;
            _goodPool = GameplayContext.Instance.Model.GoodPool;
            _goodResources = ResourceManager.Instance.GoodResources;

            _tickingService.DayPassed += OnDayPassed;
            _town.TradeCompleted += OnTradeCompleted;
            _town.DevelopmentManager.Tier.Observe(OnTownTierChanged, false);
            _town.DevelopmentManager.DevelopmentScore.Observe(OnDevelopmentChanged, false);
            _town.Missions.GoodSelectorChanged += OnGoodSelectorChanged;

            ResetAvailableGoods();
        }

        public void CleanUp()
        {
            _tickingService.DayPassed -= OnDayPassed;
            _town.TradeCompleted -= OnTradeCompleted;
            _town.DevelopmentManager.Tier.StopObserving(OnTownTierChanged);
            _town.Missions.GoodSelectorChanged -= OnGoodSelectorChanged;
        }

        private void OnDevelopmentChanged(float development)
        {
            if (development < 99.9f)
                return;

            if (_missionModel.Missions.Values.Any(mission => mission.Type == MissionType.UpgradeMission))
                return;

            TriggerMission(_upgradeMissionConfig, MissionType.UpgradeMission);
            _developmentManager.LockDegrowth(true);
        }

        private void OnGoodSelectorChanged()
        {
            ResetAvailableGoods();
        }

        private void OnTownTierChanged(Tier tier)
        {
            ResetAvailableGoods();
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
            TryTriggerTradeMission();
        }

        private void ResetAvailableGoods()
        {
            // start with all available goods on the map
            _availableGoods = _goodPool.AllAvailableGoods.ToHashSet();
            // remove from wrong tiers
            var goodsOfWrongTier = _goodResources.ResourceData
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

        private void ValidateOngoingMissions()
        {
            foreach (var mission in _missionModel.Missions.Values.ToArray())
            {
                mission.ValidateDate(_gameDate);
            }
        }

        private void TryTriggerTradeMission()
        {
            if (_missionModel.Missions.Count >= _tradeMissionConfig.MaxMissionCount)
                return;

            var isMissionTriggered = RandomUtility.GetBool(_tradeMissionConfig.DailyMissionChance);
            if (!isMissionTriggered)
                return;

            TriggerMission(_tradeMissionConfig, MissionType.TradeMission);
        }

        private void TriggerMission(IMissionConfigData config, MissionType type)
        {
            if (_availableGoods.IsEmpty())
                return;

            var missionGood = _availableGoods.GetRandom();
            var mission = new Mission(
                missionGood,
                config.Volume,
                _gameDate + config.LengthInDays,
                type,
                config.GetReward(),
                config.GetPenalty());

            mission.ValidateDate(_gameDate);

            EnableMission(mission);
        }

        private void EnableMission(Mission mission)
        {
            Debug.Log($"Mission in {_town.Name}: {mission}");
            
            _missionModel.AddMission(mission);

            mission.MissionFailed += OnMissionFailed;
            mission.MissionSucceeded += OnMissionSucceeded;

            // remove from available goods to ensure no duplicate missions
            _availableGoods.Remove(mission.Good);

            var notification = new MissionStartedNotification(_town, mission);
            _notificationService.PostNotification(notification);
        }

        private void DisableMission(Mission mission)
        {
            mission.MissionFailed -= OnMissionFailed;
            mission.MissionSucceeded -= OnMissionSucceeded;

            // add good back into available pool (if of correct tier)
            var goodTier = _goodResources.ResourceData[mission.Good].Tier;
            if (goodTier == _town.Tier.Value)
            {
                _availableGoods.Add(mission.Good);
            }

            _missionModel.RemoveMission(mission);
        }

        private void OnMissionSucceeded(Mission mission)
        {
            if (mission.Type == MissionType.UpgradeMission)
            {
                _developmentManager.LockDegrowth(false);
            }

            _resultHandler.Handle(mission.Reward);

            DisableMission(mission);
        }

        private void OnMissionFailed(Mission mission)
        {
            if (mission.Type == MissionType.UpgradeMission)
            {
                _developmentManager.LockDegrowth(false);
            }

            var notification = new MissionFailedNotification(_town, mission);
            _notificationService.PostNotification(notification);
            _resultHandler.Handle(mission.Penalty);


            DisableMission(mission);
        }
    }
}