using System;
using Common.Infrastructure;
using Features.Towns;

namespace Common.Camera
{
    public sealed class CameraService : IService
    {
        public event Action<Town> FocusCameraRequested;

        public void Initialize() { }
        public void CleanUp() { }

        public void FocusCamera(Town town)
        {
            FocusCameraRequested?.Invoke(town);
        }
    }
}