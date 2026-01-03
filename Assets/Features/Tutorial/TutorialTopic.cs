using System;

namespace Features.Tutorial
{
    public enum TutorialTopic
    {
        Intro = 0,
        Controls = 1,
        Retinue = 2,
        [Obsolete]
        Caravan = 3,
        Development = 4,
        Town = 5,
        [Obsolete]
        Reputation = 6,
        Production = 7
    }
}