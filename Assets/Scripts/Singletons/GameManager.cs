using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[Serializable]
public enum GameLevelIndex
{
    MAIN_MENU_PRE,
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

    private bool isLoadingLevel = false;

    [SerializeField] public GameLevelDictionary gameLevels;

    GameLevel currentGameLevel = null;

    private List<AsyncOperation> operations = new List<AsyncOperation> {};
    [SerializeField] private Animator sceneTransition = null;

    GameManager()
    {
        instance = this;
    }

    private void Start()
    {
#if UNITY_EDITOR
        foreach (var index in gameLevels.Values)
            if (GameObject.FindWithTag(index.tagIdentifier))
                currentGameLevel = index;
#endif
        if (currentGameLevel == null)
            Debug.LogError("Failed to find a game object with current level's tag identifier");
#if !UNITY_EDITOR
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevelsDictionary[GameLevelIndex.PRE_MAIN_MENU].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            currentGameLevel = gameLevelsDictionary[GameLevelIndex.PRE_MAIN_MENU];
#endif
    }

    public static void LoadLevelImmidiate(GameLevelIndex index)
    {
        if(!instance.isLoadingLevel)
        {
            Debug.LogWarning("[GameManager.LoadLevelImmidiate]: a level is currently being loaded");
            return;
        }

        instance.StartCoroutine(instance.LoadLevelImmidiateImpl(index));
    }

    private IEnumerator LoadLevelImmidiateImpl(GameLevelIndex index)
    {
        List<AsyncOperation> operations = new List<AsyncOperation>{};

        if (currentGameLevel)
        {
            foreach (var section in gameLevels[index].sections)
                try {
                    operations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(section.fullPath));
                } catch (ArgumentException e) { continue; } /* Scene to unload is invalid */

            operations.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentGameLevel.fullPath));
        }

        foreach (var operation in operations)
            while (!operation.isDone)
                yield return null;

        UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevels[index].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameLevels[GameLevelIndex.LOADING_SCREEN].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive);

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
        }

        // load the requested level
        operations.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameLevels[index].fullPath, UnityEngine.SceneManagement.LoadSceneMode.Additive));
        loadingScreen.SetOperations(operations);

        while (!operations[0].isDone)
            yield return null;

        // fade out of loading screen
        sceneTransition.PlayImmediate("FadeOut");

        while(sceneTransition.IsPlaying())
            yield return null;

        // unload loading screen
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(gameLevels[GameLevelIndex.LOADING_SCREEN].fullPath);
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