using System.Collections.Generic;
using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinueMiniUI : MonoBehaviour
    {
        private readonly Dictionary<CompanionType, RetinueMiniProgressGroup> _progressGroupDict = new();

        public void Initialize()
        {
            var progressGroups = GetComponentsInChildren<RetinueMiniProgressGroup>();

            foreach (var group in progressGroups)
            {
                group.Initialize();
                _progressGroupDict.Add(group.CompanionType, group);
            }
        }
        
        public void SetProgress(CompanionType companionType, int level)
        {
            _progressGroupDict[companionType].SetProgress(level);
        }
    }
}