using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;


public enum GameState
{
    // add in game states
}

public class GameManager : MonoBehaviour
{
    [Serializable]
    public enum LevelIndex
    {
        INVALID,
        MAIN_MENU_PRE,
        MAIN_MENU,

        LOADING_SCREEN,

        FIRST_LEVEL,
        LAST_LEVEL,
    }


    [SerializeField] private GameLevelDictionary gameLevels;
    [SerializeField] private Animator sceneTransition;

    private static GameManager instance = null;

    private LevelIndex currentGameLevelIndex = LevelIndex.INVALID;
    private bool isLoadingLevel = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
#if UNITY_EDITOR
        // find current level index
        foreach (var level in gameLevels.Values)
        {
            if (GameObject.FindWithTag(level.tagIdentifier))
            {
                currentGameLevelIndex = level.index;
            }
        }
        // check
        if (currentGameLevelIndex == LevelIndex.INVALID)
        {
            Debug.LogError("Failed to find a game object with current level's tag identifier");
        }
#else
        // set current level index to main_menu_pre
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevels[LevelIndex.MAIN_MENU_PRE].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        currentGameLevelIndex = LevelIndex.MAIN_MENU_PRE;
#endif
    }

    public static void LoadLevelImmediate(LevelIndex index, bool fade = false)
    {
        // check
        if (instance.isLoadingLevel)
        {
            Debug.LogWarning("[GameManager.LoadLevelImmidiate]: a level is currently being loaded");
            return;
        }

        // start loading
        instance.StartCoroutine(instance.LoadLevelImmediateImpl(index, fade));
    }

    public static void LoadLevel(LevelIndex index)
    {
        // check
        if (instance.isLoadingLevel)
        {
            Debug.LogWarning("[GameManager.LoadLevelImmidiate]: a level is currently being loaded");
            return;
        }

        // start loading
        instance.StartCoroutine(instance.LoadLevelImpl(index));
    }

    private IEnumerator LoadLevelImmediateImpl(LevelIndex index, bool fade)
    {
        isLoadingLevel = true;
        List<AsyncOperation> operations = new List<AsyncOperation> { };

        if (fade)
        {
            // fade out of current level
            sceneTransition.PlayImmediate("FadeOut");
            while (sceneTransition.IsPlaying())
                yield return null;
        }

        // add unload (current level) operations
        foreach (var section in gameLevels[index].sections)
        {
            try
            {
                operations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(section.fullPath));
            }
            // ignore ArgumentException when unloading a level section that's not loaded
            catch (ArgumentException e) /* Scene to unload is invalid */
            {
                continue; // #todo: make sure the exception is "Scene to unload is invalid"
            }
        }
        operations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(gameLevels[currentGameLevelIndex].fullPath));

        // wait for current level to unload
        foreach (var operation in operations)
            while (!operation.isDone)
                yield return null;

        // load requested level
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevels[index].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        if (fade)
        {
            // fade in to new level
            sceneTransition.PlayImmediate("FadeIn");
            while (sceneTransition.IsPlaying())
                yield return null;
        }

        currentGameLevelIndex = index;
        isLoadingLevel = false;
    }

    private IEnumerator LoadLevelImpl(LevelIndex index)
    {
        isLoadingLevel = true;

        // fade out of current level
        sceneTransition.PlayImmediate("FadeOut");
        while (sceneTransition.IsPlaying())
            yield return null;

        // unload current game level
        List<AsyncOperation> unloadOperations = new List<AsyncOperation>();

        // add unload (current level) operations
        foreach (var section in gameLevels[currentGameLevelIndex].sections)
        {
            try 
            { 
                unloadOperations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(section.fullPath)); 
            }
            // ignore ArgumentException when unloading a level section that's not loaded
            catch (ArgumentException e) /* Scene to unload is invalid */
            { 
                continue; // #todo: make sure the exception is "Scene to unload is invalid"
            } 
        }
        unloadOperations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(gameLevels[currentGameLevelIndex].fullPath));

        // wait for current level to unload
        foreach (var unloadOperation in unloadOperations)
            if (!unloadOperation.isDone)
                yield return null;

        // load loading screen
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevels[LevelIndex.LOADING_SCREEN].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        // fade in to loading screen
        sceneTransition.PlayImmediate("FadeIn");
        while (sceneTransition.IsPlaying())
            yield return null;

        // instantiate async operations
        List<AsyncOperation> loadOperations = new List<AsyncOperation>
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameLevels[index].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive)
        };

        // fetch and inform loading screen manager about operations
        LoadingScreen loadingScreen = GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreen>();
        loadingScreen.SetOperations(loadOperations);

        // wait for level to load
        // #high_priority #todo: support loading level sections with level!
        while (!loadOperations[0].isDone)
            yield return null;

        // fade out of loading screen
        sceneTransition.PlayImmediate("FadeOut");
        while (sceneTransition.IsPlaying())
            yield return null;

        // unload loading screen
        AsyncOperation unloadLoadingScreen = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(gameLevels[LevelIndex.LOADING_SCREEN].fullPath);
        while (!unloadLoadingScreen.isDone)
            yield return null;

        // fade into requested level
        sceneTransition.PlayImmediate("FadeIn");

        while (sceneTransition.IsPlaying())
            yield return null;

        currentGameLevelIndex = index;
        isLoadingLevel = false;
    }

}