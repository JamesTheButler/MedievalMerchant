using NaughtyAttributes;
using UnityEngine;

namespace Features.Cheats
{
    public sealed class CheatUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private CheatUI cheatUI;

        private void Awake()
        {
            cheatUI.Initialize();
        }
    }
}