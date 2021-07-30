using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public enum GameLevelIndex
{
    PRE_MAIN_MENU, // SPLASH_SCREEN
    MAIN_MENU,

    LOADING_SCREEN,

    FIRST_LEVEL,
    LAST_LEVEL,
}

public enum GameState
{
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField] private List<GameLevel> gameLevels = new List<GameLevel>{};
    private Dictionary<GameLevelIndex, GameLevel> gameLevelsDictionary = new Dictionary<GameLevelIndex, GameLevel> { };

    GameLevel currentGameLevel = null;

    private List<AsyncOperation> operations = new List<AsyncOperation> {};
    [SerializeField] private Animator sceneTransition = null;

    private void Awake()
    {


        instance = this;
    }

    private void Start()
    {
        foreach (var level in gameLevels)
        {
            gameLevelsDictionary[level.index] = level;

            Debug.Log(level.tagIdentifier);
#if UNITY_EDITOR
            if (GameObject.FindWithTag(level.tagIdentifier))
            {
                Debug.Log("FOUND!");
                currentGameLevel = level;
            }
#endif
        }

        if (currentGameLevel == null)
            Debug.LogError("Failed to find a game object with current level's tag identifier");
#if !UNITY_EDITOR
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevelsDictionary[GameLevelIndex.PRE_MAIN_MENU].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            currentGameLevel = gameLevelsDictionary[GameLevelIndex.PRE_MAIN_MENU];
#endif
    }

    public static void LoadLevelImmidiate(GameLevelIndex index)
    {
        instance.StartCoroutine(instance.LoadLevelImmidiateImpl(index));
    }

    private IEnumerator LoadLevelImmidiateImpl(GameLevelIndex index)
    {
        List<AsyncOperation> operations = new List<AsyncOperation>{};

        if (currentGameLevel)
        {
            foreach (var section in gameLevelsDictionary[index].sections)
                try {
                    operations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(section.fullPath));
                } catch (ArgumentException e) { continue; } /* Scene to unload is invalid */

            operations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentGameLevel.fullPath));
        }

        foreach (var operation in operations)
            while (!operation.isDone)
                yield return null;

        UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevelsDictionary[index].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public static void LoadLevel(GameLevelIndex index)
    {
        instance.StartCoroutine(instance.LoadLevelImpl(index));
    }

    private IEnumerator LoadLevelImpl(GameLevelIndex index)
    {
        // fade out of current level
        sceneTransition.PlayImmediate("FadeOut");

        while(sceneTransition.IsPlaying())
            yield return null;

        // load loading screen
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevelsDictionary[GameLevelIndex.LOADING_SCREEN].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        // fade in to loading screen
        sceneTransition.PlayImmediate("FadeIn");

        while (sceneTransition.IsPlaying())
            yield return null;

        LoadingScreenManager loadingScreen = GameObject.FindWithTag("LoadingScreen").GetComponent<LoadingScreenManager>();

        // unload current game level
        if (currentGameLevel)
        {
            List<AsyncOperation> operations = new List<AsyncOperation> { };

            foreach (var section in currentGameLevel.sections)
                try {
                    operations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(section.fullPath));
                } catch (ArgumentException e) { continue; } /* Scene to unload is invalid */

            operations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentGameLevel.fullPath));

            loadingScreen.SetPhase(LoadingScreenPhase.Unloading, operations);

            foreach (var operation1 in operations)
                while (!operation1.isDone)
                {
                    Debug.Log("Async operation unload: " + operation1.progress);
                    yield return null;
                }
        }

        // load the requested level
        operations = new List<AsyncOperation> {};
        operations.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameLevelsDictionary[index].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive));
        loadingScreen.SetPhase(LoadingScreenPhase.Loading, operations);

        while (!operations[0].isDone)
        {
            Debug.Log("Async operation load: " + operations[0].progress);
            yield return null;
        }

        // fade out of loading screen
        sceneTransition.PlayImmediate("FadeOut");

        while(sceneTransition.IsPlaying())
            yield return null;

        // unload loading screen
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(gameLevelsDictionary[GameLevelIndex.LOADING_SCREEN].fullPath);
        while (!operation.isDone)
        {
            Debug.Log("Async operation unload: " + operation.progress);
            yield return null;
        }

        // fade into requested level
        sceneTransition.PlayImmediate("FadeIn");

        while(sceneTransition.IsPlaying())
            yield return null;

    }

    //
    //static public void LoadLevelSection(GameLevelIndex section)
    //{
    //    LevelManager.LoadLevelSection(section);
    //}
    //
    //static public void UnloadLevelSection(GameLevelIndex section)
    //{
    //    LevelManager.UnloadLevelSection(section);
    //}
    //
    //private IEnumerator LoadNewLeveLCoroutine(int levelIndex)
    //{
    //    // start loading the loading screen and fade out
    //    instance.operations.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)GameLevelIndex.LOADING_SCREEN, UnityEngine.SceneManagement.LoadSceneMode.Additive));
    //    LevelManager.FadeOut();
    //
    //    // wait for all the operations to end
    //    foreach (var operation in instance.operations)
    //        if (!operation.isDone)
    //            yield return null;
    //
    //    // Fade into loading screen and start loading the levels
    //    LevelManager.FadeIn();
    //    instance.operations.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive));
    //
    //    foreach(var operation in instance.operations)
    //        if (!operation.isDone)
    //        {
    //            Debug.Log(operation.progress);
    //            yield return null;
    //        }
    //
    //    // Unload the loading screen and fade out, then fade in again
    //    instance.operations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)GameLevelIndex.LOADING_SCREEN));
    //    LevelManager.FadeOut();
    //    LevelManager.FadeIn();
    //}

}