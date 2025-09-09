using System;
using Data;
using Data.Configuration;
using Data.Setup;
using NaughtyAttributes;
using UnityEngine;

namespace UI
{
    public sealed class ZonePopup : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject zoneGoodPrefab;

        [SerializeField, Required]
        private GameObject zoneGoodContainer;

        public void Reset()
        {
            foreach (Transform zoneGood in zoneGoodContainer.transform)
            {
                Destroy(zoneGood.gameObject);
            }
        }

        public void AddGood(Good tier1, Good tier2)
        {
            var goodObject = Instantiate(zoneGoodPrefab, zoneGoodContainer.transform);
            var zoneGood = goodObject.GetComponent<ZoneGood>();
            zoneGood.SetTier1Good(tier1);
            zoneGood.SetTier2Good(tier2);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}