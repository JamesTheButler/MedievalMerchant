using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using UnityEngine;
using UnityEngine.Events;

namespace Common.UI
{
    public sealed class BackendPanelRequestReceiver : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent requestReceived;

        [SerializeField]
        private UIPanel panel;

        private void Start()
        {
            GameplayContext.Instance.Services.UIBridgeService.PanelOpenedFromBackEnd += HandleRequest;
        }

        private void HandleRequest(UIPanel requestedPanel)
        {
            if (requestedPanel != panel)
                return;

            requestReceived.Invoke();
        }
    }
}