using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinueMiniUI : MonoBehaviour
    {
        private Dictionary<CompanionType, RetinueMiniProgressGroup> _progressGroupDict;

        private void Awake()
        {
            var progressGroups = GetComponentsInChildren<RetinueMiniProgressGroup>();
            _progressGroupDict = progressGroups.ToDictionary(group => group.CompanionType, group => group);
        }

        public void SetProgress(CompanionType companionType, int level)
        {
            _progressGroupDict[companionType].SetProgress(level);
        }
    }
}