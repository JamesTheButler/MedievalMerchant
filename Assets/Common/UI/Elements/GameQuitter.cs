using UnityEditor;
using UnityEngine;

namespace Common.UI.Elements
{
    public sealed class GameQuitter : MonoBehaviour
    {
        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}