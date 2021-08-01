using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[Serializable]
public enum GameLevelIndex
{
    INVALID,
    MAIN_MENU_PRE,
    MAIN_MENU,

    LOADING_SCREEN,

    FIRST_LEVEL,
    LAST_LEVEL,
}

public enum GameState
{
    // add in the game states
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameLevelDictionary gameLevels;
    [SerializeField] private Animator sceneTransition;

    private static GameManager instance = null;

    private bool isLoadingLevel = false;
    private GameLevelIndex currentGameLevelIndex = GameLevelIndex.INVALID;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
#if UNITY_EDITOR
        foreach (var level in gameLevels.Values)
        {
            if (GameObject.FindWithTag(level.tagIdentifier))
            {
                currentGameLevelIndex = level.index;
            }
        }

        if (currentGameLevelIndex == GameLevelIndex.INVALID)
        { 
            Debug.LogError("Failed to find a game object with current level's tag identifier");
        }
#else
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevels[GameLevelIndex.MAIN_MENU_PRE].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            currentGameLevelIndex = GameLevelIndex.MAIN_MENU_PRE;
#endif
    }

    public static void LoadLevelImmediate(GameLevelIndex index, bool fade = false)
    {
        if(instance.isLoadingLevel)
        {
            Debug.LogWarning("[GameManager.LoadLevelImmidiate]: a level is currently being loaded");
            return;
        }

        instance.StartCoroutine(instance.LoadLevelImmediateImpl(index, fade));
    }

    public static void LoadLevel(GameLevelIndex index)
    {
        if (instance.isLoadingLevel)
        {
            Debug.LogWarning("[GameManager.LoadLevelImmidiate]: a level is currently being loaded");
            return;
        }

        instance.StartCoroutine(instance.LoadLevelImpl(index));
    }

    private IEnumerator LoadLevelImmediateImpl(GameLevelIndex index, bool fade)
    {
        isLoadingLevel = true;
        List<AsyncOperation> operations = new List<AsyncOperation>{};

        // fade out of current level
        if (fade)
        {
            sceneTransition.PlayImmediate("FadeOut");
            while (sceneTransition.IsPlaying())
                yield return null;
        }

            foreach (var section in gameLevels[index].sections)
            {
                try
                {
                    operations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(section.fullPath));
                }
                catch (ArgumentException e) /* Scene to unload is invalid */
                { 
                    continue;
                }
            }

            operations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(gameLevels[currentGameLevelIndex].fullPath));


        foreach (var operation in operations)
            while (!operation.isDone)
                yield return null;

        foreach (var operation in operations)
        {
            while (!operation.isDone)
            {
                yield return null;
            }
        }


        UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevels[index].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        if (fade)
        {
            sceneTransition.PlayImmediate("FadeIn");
            while (sceneTransition.IsPlaying())
                yield return null;
        }

        currentGameLevelIndex = index;
        isLoadingLevel = false;
    }

    private IEnumerator LoadLevelImpl(GameLevelIndex index)
    {
        isLoadingLevel = true;

        // fade out of current level
        sceneTransition.PlayImmediate("FadeOut");
        while(sceneTransition.IsPlaying())
            yield return null;

        // unload current game level
        List<AsyncOperation> unloadOperations = new List<AsyncOperation>();

        // unload game level sections
        foreach (var section in gameLevels[currentGameLevelIndex].sections)
            try { unloadOperations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(section.fullPath)); }
            catch (ArgumentException e) { continue; } /* Scene to unload is invalid */

        // unload game level itself
        unloadOperations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(gameLevels[currentGameLevelIndex].fullPath));

        // wait for the current level to unload
        foreach (var unloadOperation in unloadOperations)
            if (!unloadOperation.isDone)
                yield return null;

        // load loading screen
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevels[GameLevelIndex.LOADING_SCREEN].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        // fade in to loading screen
        sceneTransition.PlayImmediate("FadeIn");
        while (sceneTransition.IsPlaying())
            yield return null;


        // instantiate async operations
        List<AsyncOperation> loadOperations = new List<AsyncOperation> 
        { 
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameLevels[index].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive)
        };

        // fetch and inform the loading screen manager about the operations
        LoadingScreenManager loadingScreen = GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreenManager>();
        loadingScreen.SetOperations(loadOperations);

        // wait for the level to load
        // #high_priority #todo: support loading level sections with level!
        while (!loadOperations[0].isDone)
            yield return null;

        // fade out of loading screen
        sceneTransition.PlayImmediate("FadeOut");
        while(sceneTransition.IsPlaying())
            yield return null;

        // unload loading screen
        AsyncOperation unloadLoadingScreen = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(gameLevels[GameLevelIndex.LOADING_SCREEN].fullPath);
        while (!unloadLoadingScreen.isDone)
            yield return null;

        // fade into requested level
        sceneTransition.PlayImmediate("FadeIn");

        while(sceneTransition.IsPlaying())
            yield return null;

        currentGameLevelIndex = index;
        isLoadingLevel = false;
    }

}