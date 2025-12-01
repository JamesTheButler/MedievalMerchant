using Infrastructure;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Player
{
    public sealed class PlayerInventoryHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private PlayerInventoryUI playerInventoryUI;

        private void Start()
        {
            var player = GameplayContext.Model.Player;
            playerInventoryUI.Bind(player);
        }

        private void OnDestroy()
        {
            playerInventoryUI.Unbind();
        }
    }
}