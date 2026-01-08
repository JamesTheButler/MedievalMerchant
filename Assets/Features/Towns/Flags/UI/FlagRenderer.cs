using System;
using Common.Infrastructure;
using Features.Towns.Flags.Config;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Flags.UI
{
    public sealed class FlagRenderer : MonoBehaviour
    {
        [SerializeField, Required]
        private Image flagImage, goodImage;

        private readonly Lazy<FlagResources> _flagConfig = new(() => ResourceManager.Instance.FlagResources);

        public void SetFlag(FlagInfo info)
        {
            var data = _flagConfig.Value.GetData(info);
            flagImage.sprite = data.Flag;
            goodImage.sprite = data.RegionIcon;
            goodImage.color = data.IconColor;
        }
    }
}