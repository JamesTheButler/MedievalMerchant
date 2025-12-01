using Infrastructure;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Levels.Logic
{
    public sealed class LevelEvents : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent levelWon;

        [SerializeField]
        private UnityEvent levelLost;

        private void Awake()
        {
            GameplayContext.Systems.LevelConditionManager.LevelWon += levelWon.Invoke;
            GameplayContext.Systems.LevelConditionManager.LevelLost += levelLost.Invoke;
        }
    }
}