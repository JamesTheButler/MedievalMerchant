using Common.Utility;
using UnityEngine;

namespace Features.Towns.Reputation.Data
{
    [CreateAssetMenu(
        fileName = nameof(ReputationResources),
        menuName = AssetMenu.ResourceFolder + nameof(ReputationResources))]
    public sealed class ReputationResources : ScriptableObject
    {
        [field: SerializeField]
        public Sprite HappyIcon { get; private set; }

        [field: SerializeField]
        public Sprite UnhappyIcon { get; private set; }
    }
}