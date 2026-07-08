using System.Collections.Generic;
using Common.Infrastructure.Gameplay;
using Common.UI.Elements.Panels;
using Features.Player.Caravan.Logic;
using Features.Player.Caravan.UI;
using UnityEngine;

namespace Features.Player.Camp.UI
{
    public sealed class CampsiteCartsPanelUI : DynamicPanel
    {
        [SerializeField]
        private List<CartStatsUI> cartUis;

        private CaravanUpgrader _caravanUpgrader;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _caravanUpgrader = new CaravanUpgrader();
            var carts = GameplayContext.Instance.Model.Player.CaravanManager.Carts;

            for (var i = 0; i < cartUis.Count; i++)
            {
                var cartId = i;
                cartUis[i].Bind(
                    carts[cartId],
                    cartId,
                    () => _caravanUpgrader.RequestUpgrade(cartId),
                    () => _caravanUpgrader.RequestUpgrade(cartId));
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
    }
}
