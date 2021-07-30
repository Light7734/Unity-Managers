using UnityEngine;

using System.Collections.Generic;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    [SerializeField] private Animator sceneTransition = null;

    private List<int> cachedScenesIndex = new List<int> {};

    private GameLevel currentLevel = null;

    private bool isFadingIn = false, isFadingOut = false;

    bool isLoading = false;

    private List<AsyncOperation> asyncOperations = new List<AsyncOperation> {};

    private void Awake()
    {
        instance = this;
        // UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

    }

    public void LoadLevel(string name)
    {
        if (currentLevel)
            UnloadCurrentLevel();
    }

    public void LoadLevelSection(string name)
    {
    }

    public void UnloadLevelSection(string name)
    {
    }

    public void UnloadCurrentLevel()
    {
    }

    // public static void Load(GameScene scene)
    // {
    //     if(instance.isLoading)
    //     {
    //         Debug.LogError("[SceneManager.Load]: a scene is currently being loaded");
    //         return;
    //     }
    // 
    //     instance.StartCoroutine(instance.LoadCoroutine(scene));
    // }
    // 
    // private IEnumerator LoadCoroutine(GameScene scene)
    // {
    //     isLoading = true;
    // 
    //     if (currentScene != null)
    //         asyncOperations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)currentScene));
    // 
    //     asyncOperations.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)GameScene.LOADING_SCREEN, UnityEngine.SceneManagement.LoadSceneMode.Additive));
    // 
    //     foreach (var operation in asyncOperations)
    //         if (!operation.isDone)
    //             yield return null;
    //         else
    //             asyncOperations.Remove(operation);
    // 
    //     asyncOperations.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)scene, UnityEngine.SceneManagement.LoadSceneMode.Additive));
    // 
    //     yield return null;
    // }
    // 
    // public static void LoadSceneSection(GameScenesSection section)
    // {
    // }
    // 
    // public static void UnloadSceneSection(GameScenesSection section)
    // {
    // }
    // public static void FadeOut()
    // {
    //     instance.sceneTransition.PlayImmediate("FadeOut");
    // }
    // 
    // public static void FadeIn()
    // {
    //     instance.sceneTransition.PlayImmediate("FadeIn");
    // }
    // 
    // 
    // public static void LoadScene(int buildIndex)
    // {
    //     if (instance.isFadingOut)
    //         return;
    // 
    //     instance.cachedScenesIndex.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    //     instance.StartCoroutine(instance.LoadSceneDefered(buildIndex, 0));
    // }
    // 
    // public static void LoadScene(string name)
    // {
    //     if (instance.isFadingOut)
    //         return;
    // 
    //     instance.cachedScenesIndex.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    //     instance.StartCoroutine(instance.LoadSceneDefered(name, 0));
    // }
    // 
    // public static void LoadNextScene()
    // {
    //     LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    // }
    // 
    // public static void LoadPrevScene()
    // {
    //     LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1);
    // }
    // 
    // public static void LoadFirstCachedScene()
    // {
    //     if (instance.cachedScenesIndex.IsEmpty())
    //     {
    //         UnityEngine.Debug.LogError("SceneManager.LoadFirstCachedScene: cachedScenes is empty");
    //         return;
    //     }
    // 
    //     LoadScene(instance.cachedScenesIndex[0]);
    // }
    // 
    // public static void LoadLastCachedScene()
    // {
    //     if (instance.cachedScenesIndex.IsEmpty())
    //     {
    //         UnityEngine.Debug.LogError("SceneManager.LoadLastCachedScene: cachedScenes is empty");
    //         return;
    //     }
    // 
    //     LoadScene(instance.cachedScenesIndex.Last());
    // }
    // 
    // public static void LoadFirstScene()
    // {
    //     LoadScene(0);
    // }
    // 
    // public static void LoadLastScene()
    // {
    //     LoadScene(UnityEngine.SceneManagement.SceneManager.sceneCount - 1);
    // }
    // 
    // public static List<int> GetCachedScenes()
    // {
    //     return instance.cachedScenesIndex;
    // }
    // 
    // public static void SetSceneTransition(Animator animator)
    // {
    //     instance.sceneTransition = animator;
    // }
    // 
    // public static void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    // {
    //     Debug.Log("The scene \"" + scene.name + "\" loaded with mode: " + mode.ToString());
    // 
    //     if (instance.sceneTransition)
    //         instance.sceneTransition.PlayImmediate("FadeIn");
    // }
    // 
    // private IEnumerator LoadSceneDefered(string name, float waitDuration)
    // {
    //     instance.isFadingOut = true;
    // 
    //     // wait for previous transitions to end
    //     while (instance.isFadingIn)
    //         yield return null;
    //     instance.isFadingIn = false;
    // 
    //     instance.sceneTransition.PlayImmediate("FadeOut");
    //     while (instance.sceneTransition.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
    //         yield return null;
    // 
    //     UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    // 
    //     instance.isFadingOut = false;
    //     instance.isFadingIn = true;
    // 
    //     instance.sceneTransition.PlayImmediate("FadeIn");
    //     while (instance.sceneTransition.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
    //         yield return null;
    // 
    //     instance.isFadingIn = false;
    // }
    // 
    // private IEnumerator LoadSceneDefered(int buildIndex, float waitDuration)
    // {
    //     instance.isFadingOut = true;
    // 
    //     // wait for previous transitions to end
    //     while (instance.isFadingIn)
    //         yield return null;
    //     instance.isFadingIn = false;
    // 
    //     instance.sceneTransition.PlayImmediate("FadeOut");
    //     while (instance.sceneTransition.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
    //         yield return null;
    // 
    //     UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    // 
    //     instance.isFadingOut = false;
    //     instance.isFadingIn = true;
    // 
    //     instance.sceneTransition.PlayImmediate("FadeIn");
    //     while (instance.sceneTransition.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
    //         yield return null;
    // 
    //     instance.isFadingIn = false;
    // }
}