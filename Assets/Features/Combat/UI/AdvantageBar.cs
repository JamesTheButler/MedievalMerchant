using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Features.Combat.Data;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Features.Combat.UI
{
    public sealed class AdvantageBar : MonoBehaviour
    {
        [SerializeField, Required]
        private Image fill;

        [SerializeField, Required]
        private TMP_Text leftNameText, rightNameText, captionText;

        [SerializeField]
        private LocalizedString mutualDestructionCaption,
            evenlyMatchedCaption,
            slightlyAheadCaption,
            aheadCaption,
            dominatingCaption,
            guardsName,
            banditsName;

        private Logic.Combat _combat;
        private CombatConfig _config;

        private readonly Bindings _bindings = new();

        public void Bind(Logic.Combat combat)
        {
            Unbind();

            if (combat == null)
                return;

            _combat = combat;
            _config = ConfigurationManager.Configurations.CombatConfig;

            _bindings.Track(_combat.GuardHealthShare.Observe(OnHealthShareChanged));
        }

        public void Unbind()
        {
            _bindings.Unbind();
            _combat = null;
        }

        private void OnHealthShareChanged(float share)
        {
            fill.fillAmount = share;
            captionText.text = ResolveCaption(share);
        }

        private string ResolveCaption(float share)
        {
            if (!_combat.Player.IsAlive && !_combat.Bandits.IsAlive)
                return mutualDestructionCaption.GetLocalizedString();

            // share comes from 0-1; we normalize from -1 to 1
            var lead = Mathf.Abs(share - 0.5f) * 2f;

            if (lead < _config.EvenlyMatchedLead)
                return evenlyMatchedCaption.GetLocalizedString();

            var args = new
            {
                Side = share > 0.5f ? guardsName.GetLocalizedString() : banditsName.GetLocalizedString(),
            };

            if (lead < _config.SlightlyAheadLead)
                return slightlyAheadCaption.GetLocalizedString(args);

            return lead < _config.AheadLead
                ? aheadCaption.GetLocalizedString(args)
                : dominatingCaption.GetLocalizedString(args);
        }

        private void OnDestroy()
        {
            Unbind();
        }
    }
}
