using UnityEngine;

public class AudioSourcePool
{
    private static AudioSourcePool instance = null;

    GameObject poolGameObject = new GameObject("AudioSourcePool");

    // SoundTracks
    GameObject[] gameObjectsST = new GameObject[4];
    AudioSource[] audioSourcesST = new AudioSource[4];

    // SFX
    GameObject[] gameObjects = new GameObject[28];
    AudioSource[] audioSources = new AudioSource[28];

    private AudioSourcePool()
    {
        GameObject.DontDestroyOnLoad(poolGameObject);

        // SoundTrack
        for (int i = 0; i < 4; i++)
        {
            gameObjectsST[i] = new GameObject("AudioSourceST" + i);
            gameObjectsST[i].transform.parent = poolGameObject.transform;

            audioSourcesST[i] = gameObjectsST[i].AddComponent<AudioSource>();
            audioSourcesST[i].playOnAwake = false;
            audioSourcesST[i].loop = true;
        }

        // SFX
        for (int i = 0; i < 28; i++)
        {
            gameObjects[i] = new GameObject("AudioSource" + i);
            gameObjects[i].transform.parent = poolGameObject.transform;

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

    public static AudioSource GetAudioSourceST()
    {
        for (int i = 0; i < 4; i++)
            if (!instance.audioSourcesST[i].isPlaying)
                return instance.audioSourcesST[i];

        Debug.LogWarning("AudioSource::GetAudioSourceST: No free audio source");
        return null;
    }

    public static void RecollectChildren(Transform trasnform)
    {
        for (int i = 0; i < 28; i++)
            if (instance.gameObjects[i].transform.parent == trasnform)
                instance.gameObjects[i].transform.parent = instance.poolGameObject.transform;
    }

    public static void PrepareForSceneChange()
    {
        for (int i = 0; i < 28; i++)
        {
            instance.audioSources[i].Stop();
            instance.gameObjects[i].transform.parent = instance.poolGameObject.transform;
        }
    }

    public static bool Init()
    {
        if (instance != null)
            return false;

        instance = new AudioSourcePool();
        return instance != null;
    }
}