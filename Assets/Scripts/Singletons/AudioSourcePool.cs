using UnityEngine;

public class AudioSourcePool
{
    private static AudioSourcePool instance = null;

    GameObject poolGameObject = new GameObject("AudioSourcePool");
    GameObject[] gameObjects = new GameObject[32];
    AudioSource[] audioSources = new AudioSource[32];

    private AudioSourcePool()
    {
        GameObject.DontDestroyOnLoad(poolGameObject);

        for (int i = 0; i < 32; i++)
        {
            gameObjects[i] = new GameObject("AudioSource" + i);
            gameObjects[i].transform.parent = poolGameObject.transform;

            GameObject.DontDestroyOnLoad(gameObjects[i]);

            audioSources[i] = gameObjects[i].AddComponent<AudioSource>();
            audioSources[i].playOnAwake = false;
        }
    }

    public static AudioSource GetAudioSource(Transform transform)
    {
        for (int i = 0; i < 32; i++)
            if (!instance.audioSources[i].isPlaying)
            {
                instance.gameObjects[i].transform.parent = transform;
                instance.gameObjects[i].transform.position = transform.position;
                return instance.audioSources[i];
            }

        Debug.LogWarning("AudioSource::GetAudioSource: No free audio source");
        return null;
    }

    public static void RecollectChildren(Transform trasnform)
    {
        for (int i = 0; i < 32; i++)
            if (instance.gameObjects[i].transform.parent == trasnform)
                instance.gameObjects[i].transform.parent = instance.poolGameObject.transform;
    }

    public static void PrepareForSceneChange()
    {
        for (int i = 0; i < 32; i++)
            instance.gameObjects[i].transform.parent = instance.poolGameObject.transform;
    }

    public static bool Init()
    {
        if (instance != null)
            return false;

        instance = new AudioSourcePool();
        return instance != null;
    }
}