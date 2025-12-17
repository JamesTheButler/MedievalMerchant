using Infrastructure;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Player
{
    public sealed class PlayerInventoryUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private PlayerInventoryUI playerInventoryUI;

        private void Start()
        {
            var player = GameplayContext.Instance.Model.Player;
            playerInventoryUI.Bind(player);
        }

        private void OnDestroy()
        {
            playerInventoryUI.Unbind();
        }
    }
}