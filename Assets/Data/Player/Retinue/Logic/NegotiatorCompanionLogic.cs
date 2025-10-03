using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class NegotiatorCompanionLogic : ICompanionLogic
    {
        private readonly NegotiatorCompanionData _configData;

        public NegotiatorCompanionLogic(NegotiatorCompanionData configData)
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