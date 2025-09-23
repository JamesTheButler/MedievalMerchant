using Levels;
using TMPro;
using UnityEngine;

namespace UI.StartMenu
{
    public sealed class LevelInfoBox : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text nameText;

        public void Setup(LevelInfo levelInfo)
        {
            nameText.text = levelInfo.LevelName;
            gameObject.SetActive(true);
        }

        public void Clear()
        {
            nameText.text = string.Empty;
            gameObject.SetActive(false);
        }
    }
}