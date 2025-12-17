using Features.Player.Retinue.Logic;
using Infrastructure;
using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinueMiniUIHandler : MonoBehaviour
    {
        [SerializeField]
        private RetinueMiniUI retinueMiniUI;

        private RetinueManager _retinueManager;

        private void Start()
        {
            _retinueManager = GameplayContext.Instance.Model.Player.RetinueManager;
            foreach (var (companion, levelObservable) in _retinueManager.CompanionLevels)
            {
                levelObservable.Observe(level => OnCompanionLevelChanged(companion, level));
            }
        }

        private void OnCompanionLevelChanged(CompanionType companion, int level)
        {
            retinueMiniUI.SetProgress(companion, level);
        }
    }
}