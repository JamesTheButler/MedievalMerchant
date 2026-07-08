using System.Collections.Generic;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI.Elements.Panels;
using Common.Utility;
using Features.Player.Caravan.Logic;
using Features.Player.Caravan.UI;
using UnityEngine;

namespace Features.Player.Camp.UI
{
    public sealed class CampsiteCartsPanelUI : DynamicPanel
    {
        [SerializeField]
        private List<CartStatsUI> cartUis;

        private readonly Dictionary<int, IBinding> _cartUnlockBindings = new();

        private CaravanUpgrader _caravanUpgrader;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _caravanUpgrader = new CaravanUpgrader();

            var carts = GameplayContext.Instance.Model.Player.CaravanManager.Carts;

            for (var i = 0; i < cartUis.Count; i++)
            {
                var cartId = i; // to capture value of i, because of lambdas for the upgrader

                var cart = carts[cartId];
                var cartUI = cartUis[cartId];

                cartUI.Bind(
                    cart,
                    cartId,
                    () => _caravanUpgrader.RequestUpgrade(cartId),
                    () => _caravanUpgrader.RequestUpgrade(cartId));

                var isEnabled = cartId == 0 || cart.Level > 0 || carts[cartId - 1].Level > 0;
                cartUI.gameObject.SetActive(isEnabled);

                if (isEnabled)
                    continue;

                // observe previous cart to unlock this cart
                var previousCartId = cartId - 1;
                var binding = carts[previousCartId].Level.Observe(level => OnCartUpgraded(previousCartId, level));
                _cartUnlockBindings.Add(previousCartId, binding);
            }
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }

        private void OnCartUpgraded(int cartIndex, int level)
        {
            if (level <= 0)
                return;

            var nextIndex = cartIndex + 1;
            if (nextIndex >= cartUis.Count) // upgraded cart is the last one
                return;

            cartUis[nextIndex].gameObject.SetActive(true);

            // once cart has been upgraded, the next one should be unlocked and we can unsubscribe
            if (_cartUnlockBindings.Remove(cartIndex, out var binding))
            {
                binding.Unbind();
            }
        }
    }
}