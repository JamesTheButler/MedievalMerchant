using Infrastructure;
using UnityEngine;

namespace Features.Towns
{
    public sealed class SelectionEvents : MonoBehaviour
    {
        public void Select(Town town)
        {
            GameplayContext.Selection.Select(town);
        }

        public void Deselect()
        {
            GameplayContext.Selection.Deselect();
        }
    }
}