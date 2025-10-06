using Data.Configuration;
using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public abstract class BaseCompanionLogic<TLevelData> : ICompanionLogic
        where TLevelData : CompanionLevelData
    {
        protected CompanionConfigData<TLevelData> ConfigData { get; }
        
        protected TLevelData LevelData { get; private set; }

        protected abstract CompanionType Type { get; }

        protected BaseCompanionLogic()
        {
            ConfigData = ConfigurationManager.Instance.CompanionConfig.Get<TLevelData>(Type);
        }

        public void SetLevel(int level)
        {
            LevelData = ConfigData.TypedLevels[level];
            OnLevelChanged(level);
        }

        protected abstract void OnLevelChanged(int level);
    }
}