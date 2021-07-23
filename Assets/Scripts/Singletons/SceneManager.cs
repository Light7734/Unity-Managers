using UnityEngine;

using System.Collections.Generic;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;

    private List<int> cachedScenesIndex = new List<int> { };

    [SerializeField] private Animator sceneTransition = null;

    private bool isFadingIn = false, isFadingOut = false;

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

    public static void LoadScene(int buildIndex)
    {
        if (instance.isFadingOut)
            return;

        instance.cachedScenesIndex.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        instance.StartCoroutine(instance.LoadSceneDefered(buildIndex, 0));
    }

    public static void LoadScene(string name)
    {
        if (instance.isFadingOut)
            return;

        instance.cachedScenesIndex.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        instance.StartCoroutine(instance.LoadSceneDefered(name, 0));
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
            instance.sceneTransition.PlayImmediate("FadeIn");
    }

    private IEnumerator LoadSceneDefered(string name, float waitDuration)
    {
        instance.isFadingOut = true;

        // wait for previous transitions to end
        while (instance.isFadingIn)
            yield return null;
        instance.isFadingIn = false;

        instance.sceneTransition.PlayImmediate("FadeOut");
        while (instance.sceneTransition.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        UnityEngine.SceneManagement.SceneManager.LoadScene(name);

        instance.isFadingOut = false;
        instance.isFadingIn = true;

        instance.sceneTransition.PlayImmediate("FadeIn");
        while (instance.sceneTransition.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        instance.isFadingIn = false;
    }

    private IEnumerator LoadSceneDefered(int buildIndex, float waitDuration)
    {
        instance.isFadingOut = true;

        // wait for previous transitions to end
        while (instance.isFadingIn)
            yield return null;
        instance.isFadingIn = false;

        instance.sceneTransition.PlayImmediate("FadeOut");
        while (instance.sceneTransition.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);

        instance.isFadingOut = false;
        instance.isFadingIn = true;

        instance.sceneTransition.PlayImmediate("FadeIn");
        while (instance.sceneTransition.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        instance.isFadingIn = false;
    }
}