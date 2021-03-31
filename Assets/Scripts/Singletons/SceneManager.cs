public class SceneManager
{
    private static SceneManager instance;

    public SceneManager() {}

    public static bool Init()
    {
        if (instance != null)
            return false;

        instance = new SceneManager();
        return instance != null;
    }

    public static void LoadScene(int buildIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }

    public static void LoadSceneByName(string sceneName)
    {
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
        LoadScene(0);
    }

    public static void LoadLastScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.sceneCount - 1);
    }

}