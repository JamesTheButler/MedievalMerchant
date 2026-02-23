using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Levels.Conditions.Data
{
    [Serializable]
    public abstract class ConditionData
    {
        [SerializeField]
        protected LocalizedString formatter;

        public abstract ConditionType Type { get; }
        public abstract string Description { get; }
    }
}