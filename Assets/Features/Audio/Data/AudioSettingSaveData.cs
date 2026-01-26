using System;

namespace Features.Audio.Data
{
    [Serializable]
    public record AudioSettingSaveData(int MasterVolume, int MusicVolume, int SfxVolume, int InterfaceVolume);
}