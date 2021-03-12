public class SceneManager
{
    private static SceneManager instance;

    public static void LoadScene(int buildIndex)
    {
        AudioSourcePool.PrepareForSceneChange();
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }

    public static void LoadNextScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static bool Init()
    {
        if (instance != null)
            return false;

        instance = new SceneManager();
        return instance != null;
    }

}