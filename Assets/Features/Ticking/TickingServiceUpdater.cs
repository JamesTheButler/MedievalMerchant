using Infrastructure;
using UnityEngine;

namespace Features.Ticking
{
    public sealed class TickingServiceHandler : MonoBehaviour
    {
        private TickingService _tickingService;

        private void Awake()
        {
            _tickingService = GameplayContext.Instance.Services.TickingService;
        }

        private void FixedUpdate()
        {
            _tickingService.Update(Time.deltaTime);
        }
    }
}