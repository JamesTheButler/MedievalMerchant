using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class DiplomatCompanionLogic : ICompanionLogic
    {
        private readonly DiplomatCompanionData _configData;

        public DiplomatCompanionLogic(DiplomatCompanionData configData)
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