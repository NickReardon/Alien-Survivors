
using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
