using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Levels.Conditions.UI
{
    public sealed class PreGameConditionListItem : MonoBehaviour
    {
        [SerializeField]
        private Image conditionIcon;

        [SerializeField]
        private TMP_Text conditionText;

        public void Setup(Sprite icon, string description)
        {
            conditionIcon.sprite = icon;
            conditionText.text = description;
        }
    }
}