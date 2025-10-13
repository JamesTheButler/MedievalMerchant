using System;
using Common;
using Features.Towns.Flags.Config;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Flags.UI
{
    public sealed class FlagUI : MonoBehaviour
    {
        [SerializeField, Required]
        private Image flagImage, goodImage;

        private readonly Lazy<FlagConfig> _flagConfig = new(() => ConfigurationManager.Instance.FlagConfig);

        public void SetFlag(FlagInfo info)
        {
            var data = _flagConfig.Value.GetData(info);

            flagImage.sprite = data.Flag;
            goodImage.sprite = data.GoodIcon;
            goodImage.color = data.GoodColor;
        }
    }
}