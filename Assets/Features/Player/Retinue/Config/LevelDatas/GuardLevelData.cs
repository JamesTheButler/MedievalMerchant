using System;
using System.Text;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public class GuardLevelData : CompanionLevelData
    {
        [field: SerializeField]
        public int Strength { get; private set; }

        public override string Description => new StringBuilder()
            .AppendLine($"- {Strength} combat strength")
            .ToString();
    }
}