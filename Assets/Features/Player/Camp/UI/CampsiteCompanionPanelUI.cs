using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Global;
using Common.Infrastructure.Observation;
using Common.UI.Elements.Panels;
using Common.UI.Tooltips;
using Features.Localization.Data;
using Features.Player.Retinue.Logic;
using Features.Tutorial;
using Features.Tutorial.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Player.Camp.UI
{
    public sealed class CampsiteCompanionPanelUI : DynamicPanel
    {
        [SerializeField, Required]
        private TMP_Text upkeepText;

        [SerializeField, Required]
        private ModifiableTooltipHandler tooltipHandler;

        private CampsiteCompanionPanelUiItem[] _companionGroups;

        private TutorialService _tutorialService;
        private LocalizationResources _localizationResources;
        private RetinueModel _retinueModel;
        private IBinding _upkeepBinding;

        public override void Initialize()
        {
            _tutorialService = GlobalContext.Instance.Services.TutorialService;
            _localizationResources = ResourceManager.Instance.LocalizationResources;
            _companionGroups = GetComponentsInChildren<CampsiteCompanionPanelUiItem>();

            _retinueModel = GameplayContext.Instance.Model.Player.RetinueModel;
        }

        protected override void OnOpen()
        {
            _tutorialService.TryOpenFirstTime(TutorialTopic.Companions);
            gameObject.SetActive(true);
            foreach (var group in _companionGroups)
            {
                group.Bind();
            }

            tooltipHandler.SetData(_retinueModel.Upkeep);

            _upkeepBinding = _retinueModel.Upkeep.Observe(OnUpkeepChanged);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
            foreach (var group in _companionGroups)
            {
                group.Unbind();
            }

            tooltipHandler.SetData(null);
            _upkeepBinding?.Unbind();
            _upkeepBinding = null;
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepText.text = _localizationResources.PerDay($"{upkeep:0.0}");
        }
    }
}