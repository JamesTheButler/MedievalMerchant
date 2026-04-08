using System.Collections.Generic;
using Common.Camera;
using Common.UI;
using Features.Cheats;
using Features.Levels.GameModifiers.Logic;
using Features.Map;
using Features.Notifications.Logic;
using Features.Player.Camp.Logic;
using Features.Player.Retinue.Logic;
using Features.Ticking.Logic;
using Features.Trade.Logic;

namespace Common.Infrastructure.Gameplay
{
    public sealed class GameplayServices
    {
        public TickingService TickingService { get; private set; }
        public CompanionUpgradeService CompanionUpgradeService { get; private set; }
        public CompanionDeliveryService CompanionDeliveryService { get; private set; }
        public CampsiteStorageService CampsiteStorageService { get; private set; }
        public TradeService TradeService { get; private set; }
        public GameModifierService GameModifierService { get; private set; }
        public NotificationService NotificationService { get; private set; }
        public UIBridgeService UIBridgeService { get; private set; }
        public CameraService CameraService { get; private set; }
        public CheatService Cheats { get; private set; }
        public NavigationService NavigationService { get; private set; }

        private readonly List<IService> _services = new();

        public void Initialize()
        {
            TickingService = new TickingService();
            CompanionUpgradeService = new CompanionUpgradeService();
            CompanionDeliveryService = new CompanionDeliveryService();
            CampsiteStorageService = new CampsiteStorageService();
            TradeService = new TradeService();
            GameModifierService = new GameModifierService();
            NotificationService = new NotificationService();
            UIBridgeService = new UIBridgeService();
            CameraService = new CameraService();
            Cheats = new CheatService();
            NavigationService = new NavigationService();

            _services.Add(TickingService);
            _services.Add(CompanionUpgradeService);
            _services.Add(CompanionDeliveryService);
            _services.Add(CampsiteStorageService);
            _services.Add(TradeService);
            _services.Add(GameModifierService);
            _services.Add(NotificationService);
            _services.Add(UIBridgeService);
            _services.Add(CameraService);
            _services.Add(Cheats);
            _services.Add(NavigationService);

            foreach (var service in _services)
            {
                service.Initialize();
            }
        }

        public void CleanUp()
        {
            foreach (var service in _services)
            {
                service.CleanUp();
            }

            _services.Clear();
        }
    }
}