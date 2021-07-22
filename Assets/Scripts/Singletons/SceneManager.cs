using System.Collections.Generic;

public class SceneManager
{
    private static SceneManager instance;

    private List<UnityEngine.SceneManagement.Scene> cachedScenes;

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
        instance.cachedScenes.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.buildIndex);
    }

    public static void LoadScene(int buildIndex)
    {
        instance.cachedScenes.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }

    public static void LoadSceneByName(string name)
    {
        instance.cachedScenes.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        UnityEngine.SceneManagement.SceneManager.GetSceneByName(name);
    }

    public static void LoadNextScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void LoadPrevScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1);
    }

    public static void LoadFirstCachedScene()
    {
        if (instance.cachedScenes.IsEmpty())
        {
            UnityEngine.Debug.LogError("SceneManager.LoadFirstCachedScene: cachedScenes is empty");
            return;
        }

        LoadScene(instance.cachedScenes[0]);
    }

    public static void LoadLastCachedScene()
    {
        if (instance.cachedScenes.IsEmpty())
        {
            UnityEngine.Debug.LogError("SceneManager.LoadLastCachedScene: cachedScenes is empty");
            return;
        }
            
        LoadScene(instance.cachedScenes.Last());
    }

    public static void LoadFirstScene()
    {
        LoadScene(0);
    }

    public static void LoadLastScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.sceneCount - 1);
    }

    public List<UnityEngine.SceneManagement.Scene> GetCachedScenes()
    {
        return instance.cachedScenes;
    }

}