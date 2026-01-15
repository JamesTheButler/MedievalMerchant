using System;

namespace Features.Settings.Logic
{
    [Serializable]
    public record AudioSettingSaveData(int MasterVolume, int MusicVolume, int SfxVolume, int InterfaceVolume);
}