using Common.Config;
using Features.Goods.Config;
using NaughtyAttributes;
using UnityEngine;

namespace Infrastructure
{
    [ExecuteInEditMode]
    public sealed class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance;

        [field: SerializeField, Required]
        public AvailabilityResources AvailabilityResources { get; private set; }

        [field: SerializeField, Required]
        public CaravanResources CaravanResources { get; private set; }

        [field: SerializeField, Required]
        public Colors Colors { get; private set; }

        [field: SerializeField, Required]
        public Cursors Cursors { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }

                return;
            }

            Instance = this;
        }
    }
}