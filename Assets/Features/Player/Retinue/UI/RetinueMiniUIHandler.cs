using Common.Infrastructure;
using Features.Player.Retinue.Logic;
using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinueMiniUIHandler : MonoBehaviour
    {
        [SerializeField]
        private RetinueMiniUI retinueMiniUI;

        private RetinueModel _retinueModel;

        private void Start()
        {
            _retinueModel = GameplayContext.Instance.Model.Player.RetinueModel;
            foreach (var (companion, levelObservable) in _retinueModel.CompanionLevels)
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