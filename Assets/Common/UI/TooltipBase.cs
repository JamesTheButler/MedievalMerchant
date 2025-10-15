using UnityEngine;

namespace Common.UI
{
    public abstract class TooltipBase<TData> : MonoBehaviour
    {
        public abstract void SetData(TData data);
    }
}
