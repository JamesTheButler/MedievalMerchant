using System;

namespace Features.Settings.Logic
{
    [Serializable]
    public record AudioSettings(int MasterVolume, int MusicVolume, int SfxVolume, int InterfaceVolume);
}