using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class NavigatorCompanionLogic : ICompanionLogic
    {
        private readonly NavigatorCompanionData _configData;

        public NavigatorCompanionLogic(NavigatorCompanionData configData)
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