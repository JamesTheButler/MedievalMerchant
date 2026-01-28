using NaughtyAttributes;
using UnityEngine;

namespace Common.UI.Elements
{
    public sealed class GameObjectToggler : MonoBehaviour
    {
        [SerializeField]
        private bool useSelf = true;

        [SerializeField, HideIf(nameof(useSelf))]
        private GameObject target;

        private void Awake()
        {
            target = gameObject;
        }

        public void Toggle()
        {
            target.SetActive(!target.activeSelf);
        }
    }
}