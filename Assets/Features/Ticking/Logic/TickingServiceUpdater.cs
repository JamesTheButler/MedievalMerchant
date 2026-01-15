using Common.Infrastructure.Gameplay;
using UnityEngine;

namespace Features.Ticking.Logic
{
    public sealed class TickingServiceHandler : MonoBehaviour
    {
        private TickingService _tickingService;

        private void Start()
        {
            _tickingService = GameplayContext.Instance.Services.TickingService;
        }

        private void FixedUpdate()
        {
            _tickingService.Update(Time.fixedDeltaTime);
        }
    }
}