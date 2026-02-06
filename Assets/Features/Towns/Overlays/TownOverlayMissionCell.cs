using Common.Types;
using Common.UI.Elements.Cells;
using Features.Towns.Missions;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Overlays
{
    public sealed class TownOverlayMissionCell : MonoBehaviour
    {
        [SerializeField, Required]
        private GoodCell goodCell;

        [SerializeField, Required]
        private Sprite tradeMissionBackground, upgradeMissionBackground;

        [SerializeField, Required]
        private Image background, progressBar;

        private int _totalLength;

        public void SetUp(Good good, int totalLength, MissionType missionType, int daysRemaining)
        {
            goodCell.SetGood(good);
            _totalLength = totalLength;

            background.sprite = missionType == MissionType.TradeMission
                ? tradeMissionBackground
                : upgradeMissionBackground;

            SetDaysRemaining(daysRemaining);
        }

        public void SetDaysRemaining(int daysRemaining)
        {
            progressBar.fillAmount = (float)daysRemaining / _totalLength;
        }
    }
}