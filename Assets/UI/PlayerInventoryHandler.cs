using Data;
using NaughtyAttributes;
using UnityEngine;

namespace UI
{
    public sealed class PlayerInventoryHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private PlayerInventoryUI playerInventoryUI;

        private Player _player;

        private void Start()
        {
            _player = Model.Instance.Player;

            playerInventoryUI.Bind(_player);
        }

        private void OnDestroy()
        {
            playerInventoryUI.Unbind();
        }
    }
}