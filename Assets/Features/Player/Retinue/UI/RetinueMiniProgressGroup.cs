using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinueMiniProgressGroup : MonoBehaviour
    {
        private RetinueMiniProgressElement[] _elements;

        private void Start()
        {
            _elements = GetComponentsInChildren<RetinueMiniProgressElement>();
        }

        public void SetProgress(int count)
        {
            for (var i = 0; i < _elements.Length; i++)
            {
                _elements[i].SetCompleted(i < count);
            }
        }
    }
}