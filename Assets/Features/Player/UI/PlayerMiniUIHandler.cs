using Common.Infrastructure;
using UnityEngine;

namespace Features.Player.UI
{
    public sealed class PlayerMiniUIHandler : MonoBehaviour
    {
        [SerializeField]
        private PlayerMiniUI playerMiniUI;

        private PlayerModel _player;

        private void Start()
        {
            _player = GameplayContext.Instance.Model.Player;

            playerMiniUI.SetFundsChangeTooltipTarget(_player.FundsChange);
            _player.Inventory.Funds.Observe(OnFundsChanged);
            _player.FundsChange.Observe(OnFundsChangeChanged);
        }

        private void OnFundsChangeChanged(float fundsChange)
        {
            playerMiniUI.SetFundsChange(fundsChange);
        }

        private void OnFundsChanged(float funds)
        {
            playerMiniUI.SetFunds(funds);
        }

        private void OnDestroy()
        {
            _player.FundsChange.StopObserving(OnFundsChanged);
        }
    }
}