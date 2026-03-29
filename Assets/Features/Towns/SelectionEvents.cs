using Common.Infrastructure.Gameplay;
using UnityEngine;

namespace Features.Towns
{
    public sealed class SelectionEvents : MonoBehaviour
    {
        public void Select(Town town)
        {
            GameplayContext.Instance.Selection.Select(town);
        }

        public void Deselect()
        {
            GameplayContext.Instance.Selection.Deselect();
        }

        public void SelectCamp()
        {
            GameplayContext.Instance.Selection.SelectCamp();
        }
    }
}