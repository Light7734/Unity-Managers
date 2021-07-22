using UnityEngine;

using System.Collections.Generic;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;

    private List<int> cachedScenesIndex = new List<int> { };

    [SerializeField] private Animator sceneTransition = null;

    public SceneManager() {}

    public static bool Init()
    {
        if (instance != null)
            return false;

        instance = (Instantiate(Resources.Load("SceneManager", typeof(GameObject))) as GameObject).GetComponent<SceneManager>();
        DontDestroyOnLoad(instance.gameObject);

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        return instance != null;
    }

    public static void LoadScene(UnityEngine.SceneManagement.Scene scene)
    {
        if(instance.sceneTransition)
            instance.sceneTransition.SetTrigger("FadeOut");

        instance.cachedScenesIndex.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.buildIndex);
    }

    public static void LoadScene(int buildIndex)
    {
        if (instance.sceneTransition)
            instance.sceneTransition.SetTrigger("FadeOut");

        instance.cachedScenesIndex.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }

    public static void LoadScene(string name)
    {
        if (instance.sceneTransition)
            instance.sceneTransition.SetTrigger("FadeOut");

        instance.cachedScenesIndex.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
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
        if (instance.cachedScenesIndex.IsEmpty())
        {
            UnityEngine.Debug.LogError("SceneManager.LoadFirstCachedScene: cachedScenes is empty");
            return;
        }

        LoadScene(instance.cachedScenesIndex[0]);
    }

    public static void LoadLastCachedScene()
    {
        if (instance.cachedScenesIndex.IsEmpty())
        {
            UnityEngine.Debug.LogError("SceneManager.LoadLastCachedScene: cachedScenes is empty");
            return;
        }

        LoadScene(instance.cachedScenesIndex.Last());
    }

    public static void LoadFirstScene()
    {
        LoadScene(0);
    }

    public static void LoadLastScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.sceneCount - 1);
    }

    public static List<int> GetCachedScenes()
    {
        return instance.cachedScenesIndex;
    }

    public static void SetSceneTransition(Animator animator)
    {
        instance.sceneTransition = animator;
    }

    public static void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Debug.Log("The scene \"" + scene.name + "\" loaded with mode: " + mode.ToString());

        if (instance.sceneTransition)
        {
            Debug.Log("Triggering a scene transition");
            instance.sceneTransition.SetTrigger("FadeIn");
        }
    }

}