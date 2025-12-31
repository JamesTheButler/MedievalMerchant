using System.Collections.Generic;
using Common.Camera;
using Features.Levels.GameModifiers.Logic;
using Features.Notifications.Logic;
using Features.Player.Retinue.Logic;
using Features.Ticking.Logic;
using Features.Trade.Logic;
using Features.Tutorial.Logic;

namespace Common.Infrastructure
{
    public sealed class GameplayServices
    {
        public TutorialPersistenceService TutorialPersistenceService { get; private set; }
        public TutorialService TutorialService { get; private set; }
        public TickingService TickingService { get; private set; }
        public CompanionUpgradeService CompanionUpgradeService { get; private set; }
        public TradeService TradeService { get; private set; }
        public GameModifierService GameModifierService { get; private set; }
        public NotificationService NotificationService { get; private set; }
        public UIBridgeService UIBridgeService { get; private set; }
        public CameraService CameraService { get; private set; }

        private readonly List<IService> _services = new();

        public void Initialize()
        {
            TutorialPersistenceService = new TutorialPersistenceService(GlobalContext.Instance.Services.Serializer);
            TutorialService = new TutorialService(TutorialPersistenceService);
            TickingService = new TickingService();
            CompanionUpgradeService = new CompanionUpgradeService();
            TradeService = new TradeService();
            GameModifierService = new GameModifierService();
            NotificationService = new NotificationService();
            UIBridgeService = new UIBridgeService();
            CameraService = new CameraService();

            _services.Add(TutorialPersistenceService);
            _services.Add(TutorialService);
            _services.Add(TickingService);
            _services.Add(CompanionUpgradeService);
            _services.Add(TradeService);
            _services.Add(GameModifierService);
            _services.Add(NotificationService);
            _services.Add(UIBridgeService);
            _services.Add(CameraService);

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