using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Levels.Conditions.Data
{
    [Serializable]
    public abstract class LossConditionData : ConditionData
    {
        [field: SerializeField]
        protected LocalizedString warningMessageFormatter;

        [field: SerializeField]
        protected LocalizedString gameOverMessageFormatter;

        public abstract string WarningMessage { get; }
        public abstract string GameOverMessage { get; }
    }
}