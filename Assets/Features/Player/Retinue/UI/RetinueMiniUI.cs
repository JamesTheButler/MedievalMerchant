using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinueMiniUI : MonoBehaviour
    {
        [SerializeField, SerializedDictionary]
        private SerializedDictionary<CompanionType, RetinueMiniProgressGroup> progressGroups;

        public void SetProgress(CompanionType companionType, int level)
        {
            progressGroups[companionType].SetProgress(level);
        }
    }
}