using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class GuardCompanionLogic : ICompanionLogic
    {
        private readonly GuardCompanionData _configData;

        public GuardCompanionLogic(GuardCompanionData configData)
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