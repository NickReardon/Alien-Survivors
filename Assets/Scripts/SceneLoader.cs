using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneLoader : MonoBehaviour
{


    [SerializeField]
    private string sceneName;

    #if UNITY_EDITOR
    [SerializeField]
    private SceneAsset sceneAsset;
    #endif

    // Public method to change the scene name without loading it
    public void ChangeSceneName(string newSceneName)
    {
        if (!string.IsNullOrEmpty(newSceneName))
        {
            sceneName = newSceneName;
        }
    }

    // Public method to change the scene name using a SceneAsset without loading it
    #if UNITY_EDITOR
    public void ChangeSceneUsingAsset(SceneAsset newSceneAsset)
    {
        if (newSceneAsset != null)
        {
            sceneAsset = newSceneAsset;
            sceneName = newSceneAsset.name;
        }
    }
    #endif

    // Public method to initiate scene loading
    public virtual void InitiateSceneLoad()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            StartCoroutine(LoadYourAsyncScene(sceneName));
        }
    }


    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
        else
        {
            sceneName = string.Empty;
        }
    }
    #endif
}
