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
            var conditions = GameplayContext.Instance.Model.Conditions;
            conditions.LevelWon += levelWon.Invoke;
            conditions.LevelLost += levelLost.Invoke;
        }
    }
}