public class SceneManager
{
    private static SceneManager instance;

    public static void LoadScene(int buildIndex)
    {
        AudioSourcePool.PrepareForSceneChange();
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }

    public static void LoadSceneByName(string sceneName)
    {
        AudioSourcePool.PrepareForSceneChange();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public static void LoadNextScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void LoadPrevScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1);
    }

    public static void LoadFirstScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 0);
    }

    public static bool Init()
    {
        if (instance != null)
            return false;

        instance = new SceneManager();
        return instance != null;
    }

}