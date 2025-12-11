using Infrastructure;

namespace Features.Towns.Production.Logic
{
    public sealed class TownProductionSystem : ISystem
    {
        private readonly Town _town;

        public TownProductionSystem(Town town)
        {
            _town = town;
        }

        public void Initialize() { }
        public void CleanUp() { }
    }
}