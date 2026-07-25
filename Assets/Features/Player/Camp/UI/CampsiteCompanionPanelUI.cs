using Common.Infrastructure.Global;
using Common.UI.Elements.Panels;
using Features.Tutorial;
using Features.Tutorial.Logic;

namespace Features.Player.Camp.UI
{
    public sealed class CampsiteCompanionPanelUI : DynamicPanel
    {
        private CampsiteCompanionPanelUiItem[] _companionGroups;

        private TutorialService _tutorialService;
        
        public override void Initialize()
        {
            _tutorialService = GlobalContext.Instance.Services.TutorialService;
            _companionGroups = GetComponentsInChildren<CampsiteCompanionPanelUiItem>();
        }

        protected override void OnOpen()
        {
            _tutorialService.TryOpenFirstTime(TutorialTopic.Companions);
            gameObject.SetActive(true);
            foreach (var group in _companionGroups)
            {
                group.Bind();
            }
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
            foreach (var group in _companionGroups)
            {
                group.Unbind();
            }
        }
    }
}