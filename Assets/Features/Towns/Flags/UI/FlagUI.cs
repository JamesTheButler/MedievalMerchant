using System;
using Common;
using Features.Towns.Flags.Config;
using Infrastructure;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Flags.UI
{
    public sealed class FlagUI : MonoBehaviour
    {
        [SerializeField, Required]
        private Image flagImage, goodImage;

        private readonly Lazy<FlagResources> _flagConfig = new(() => ResourceManager.Instance.FlagResources);

        public void SetFlag(FlagInfo info)
        {
            var data = _flagConfig.Value.GetData(info);

            flagImage.sprite = data.Flag;
            goodImage.sprite = data.RegionIcon;
            goodImage.color = data.GoodColor;
        }
    }
}