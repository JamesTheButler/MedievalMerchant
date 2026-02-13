using Common.Infrastructure.Gameplay;
using Features.Player.Retinue.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinueMiniUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private RetinueMiniUI retinueMiniUI;

        private RetinueModel _retinueModel;

        private void Start()
        {
            _retinueModel = GameplayContext.Instance.Model.Player.RetinueModel;
            retinueMiniUI.Initialize();
            foreach (var (companion, companionModel) in _retinueModel.Companions)
            {
                companionModel.Level.Observe(level => OnCompanionLevelChanged(companion, level));
            }
        }

        private void OnCompanionLevelChanged(CompanionType companion, int level)
        {
            retinueMiniUI.SetProgress(companion, level);
        }
    }
}