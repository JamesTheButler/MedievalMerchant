using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class ThiefCompanionLogic : ICompanionLogic
    {
        private readonly ThiefCompanionData _configData;

        public ThiefCompanionLogic(ThiefCompanionData configData)
        {
            _configData = configData;
        }

        public void ApplyLevel(int level)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveLevel(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}