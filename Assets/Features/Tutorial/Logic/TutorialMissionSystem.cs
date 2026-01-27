using Common.Infrastructure;

namespace Features.Tutorial.Logic
{
    public sealed class TutorialMissionSystem : ISystem
    {
        public void Initialize()
        {
            // if first mission is completed (i.e. 15 hay, trigger upgrade mission in HayTown)
        }

        public void CleanUp() { }
    }
}