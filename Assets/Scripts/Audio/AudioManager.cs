using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;

    public static AudioManager i
    {
        get
        {
            if (instance == null)
                instance = (Instantiate(Resources.Load("StaticPrefabs/AudioManager")) as GameObject).GetComponent<AudioManager>();
            instance.name = "__AUDIO_MANAGER__";

            DontDestroyOnLoad(instance);
            return instance;
        }
    }

    [SerializeField]
    List<AudioNameClipPair> audioClips = new List<AudioNameClipPair>();

    Dictionary<AudioName, AudioClip> audioClipsMap = new Dictionary<AudioName, AudioClip>();

    private void Awake()
    {
        foreach (AudioNameClipPair audioClip in audioClips)
        {
            audioClipsMap.Add(audioClip.name, audioClip.clip);
        }

        audioClips = null;
    }

    public AudioClip GetClip(AudioName name)
    {
        return audioClipsMap[name];
    }

    [Serializable]
    public class AudioNameClipPair
    {
        public AudioClip clip;
        public AudioName name;
    }
}