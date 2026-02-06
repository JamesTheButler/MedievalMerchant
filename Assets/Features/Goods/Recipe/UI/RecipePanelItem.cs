using Common.Infrastructure;
using Common.Types;
using Common.UI.Elements.Cells;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Goods.Recipe.UI
{
    public sealed class RecipePanelItem : MonoBehaviour
    {
        [SerializeField, Required]
        private GoodCell goodCell;
        
        [SerializeField, Required]
        private TMP_Text goodTitle;
        
        [SerializeField, Required]
        private Image tierIcon;
        
        public void Setup(Good good)
        {
            var goodResourceData = ResourceManager.Instance.GoodResources.ResourceData[good];
            var tierResources = ResourceManager.Instance.TierResources;

            goodCell.SetGood(good);
            goodTitle.text = goodResourceData.GoodName;
            tierIcon.sprite = tierResources.Icons[goodResourceData.Tier];
        }
    }
}