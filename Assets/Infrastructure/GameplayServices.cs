using Features.Tutorial.Logic;

namespace Infrastructure
{
    public sealed class GameplayServices
    {
        public TutorialService TutorialService { get; private set; } = new();

        public void Initialize()
        {
        }

        public void CleanUp()
        {
        }
    }
}