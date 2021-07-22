using System.Collections.Generic;

public class SceneManager
{
    private static SceneManager instance;

    private List<int> cachedScenesIndex = new List<int> { };

    public SceneManager() {}

    public static bool Init()
    {
        if (instance != null)
            return false;

        instance = new SceneManager();
        return instance != null;
    }

    public static void LoadScene(UnityEngine.SceneManagement.Scene scene)
    {
        instance.cachedScenesIndex.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.buildIndex);
    }

    public static void LoadSceneByBuildIndex(int buildIndex)
    {
        instance.cachedScenesIndex.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }

    public static void LoadSceneByName(string name)
    {
        instance.cachedScenesIndex.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        UnityEngine.Debug.Log("Added scene: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public static void LoadNextScene()
    {
        LoadSceneByBuildIndex(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void LoadPrevScene()
    {
        LoadSceneByBuildIndex(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1);
    }

    public static void LoadFirstCachedScene()
    {
        if (instance.cachedScenesIndex.IsEmpty())
        {
            UnityEngine.Debug.LogError("SceneManager.LoadFirstCachedScene: cachedScenes is empty");
            return;
        }

        LoadSceneByBuildIndex(instance.cachedScenesIndex[0]);
    }

    public static void LoadLastCachedScene()
    {
        if (instance.cachedScenesIndex.IsEmpty())
        {
            UnityEngine.Debug.LogError("SceneManager.LoadLastCachedScene: cachedScenes is empty");
            return;
        }
            
        LoadSceneByBuildIndex(instance.cachedScenesIndex.Last());
    }

    public static void LoadFirstScene()
    {
        LoadSceneByBuildIndex(0);
    }

    public static void LoadLastScene()
    {
        LoadSceneByBuildIndex(UnityEngine.SceneManagement.SceneManager.sceneCount - 1);
    }

    public static List<int> GetCachedScenes()
    {
        string text = "Cached Scenes Index: ";
        foreach (int sceneIndex in instance.cachedScenesIndex)
            text += sceneIndex + ", ";
        UnityEngine.Debug.Log(text);

        return instance.cachedScenesIndex;
    }

}