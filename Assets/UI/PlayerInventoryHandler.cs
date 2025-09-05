using Data;
using NaughtyAttributes;
using UnityEngine;

namespace UI
{
    public sealed class PlayerInventoryHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private PlayerInventoryUI playerInventoryUI;

        private void Start()
        {
            var player = Model.Instance.Player;
            playerInventoryUI.Bind(player);
        }

        private void OnDestroy()
        {
            playerInventoryUI.Unbind();
        }
    }
}