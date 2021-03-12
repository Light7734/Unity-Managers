using System;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public abstract class AudioEmitter : ScriptableObject
{
    public abstract void Play(AudioSource source);
}

[CreateAssetMenu(fileName = "BasicAudioEmitter", menuName = "Audio/Basic Emitter", order = 1)]
public class BasicAudioEmitter : AudioEmitter
{
    [SerializeField]
    private AudioClip clip;

    public override void Play(AudioSource source)
    {
        if (source == null)
        {
            Debug.Log("Source is invalid");
            return;
        }

        source.clip = clip;
        source.Play();
    }   
}