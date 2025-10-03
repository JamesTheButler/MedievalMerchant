using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class ArchitectCompanionLogic : ICompanionLogic
    {
        private readonly ArchitectCompanionData _configData;

        public ArchitectCompanionLogic(ArchitectCompanionData configData)
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