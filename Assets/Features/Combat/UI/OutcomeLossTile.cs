using Common.Infrastructure;
using Common.UI.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Combat.UI
{
    public sealed class OutcomeLossTile : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text sideLabel, lostCountText, totalCountText;

        public void SetLosses(string sideHeader, int unitsLost, int totalUnitCount, bool isPositiveGood)
        {
            var loc = ResourceManager.Instance.LocalizationResources.Combat;

            sideLabel.text = sideHeader;
            var lostCounterStyle = unitsLost.GetNumberStyle(isPositiveGood);
            lostCountText.text = unitsLost.ToString().WithStyle(lostCounterStyle);
            totalCountText.text = loc.UnitLossOutOf(totalUnitCount);
        }
    }
}