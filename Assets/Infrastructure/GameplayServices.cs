using Features.Player.Retinue.Logic;
using Features.Ticking;
using Features.Tutorial.Logic;

namespace Infrastructure
{
    public sealed class GameplayServices
    {
        public TutorialPersistenceService TutorialPersistenceService { get; private set; }
        public TutorialService TutorialService { get; private set; }
        public TickingService TickingService { get; private set; }
        public CompanionUpgradeService CompanionUpgradeService { get; private set; }

        public void Initialize()
        {
            TutorialPersistenceService = new TutorialPersistenceService(GlobalContext.Instance.Services.Serializer);

            TutorialService = new TutorialService(TutorialPersistenceService);
            TutorialService.Initialize();
            TickingService = new TickingService();
            TickingService.Initialize();
            CompanionUpgradeService = new CompanionUpgradeService();
            CompanionUpgradeService.Initialize();
        }

        public void CleanUp()
        {
            TutorialService.CleanUp();
            TickingService.CleanUp();
            CompanionUpgradeService.CleanUp();
        }
    }
}