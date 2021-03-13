using UnityEngine;

public abstract class AudioEmitter : ScriptableObject
{
    public abstract void Play(Transform transform);
}

[CreateAssetMenu(fileName = "BasicAudioEmitter", menuName = "Audio/Basic Emitter", order = 1)]
public class BasicAudioEmitter : AudioEmitter
{
    [SerializeField]
    private AudioClip clip;

    public override void Play(Transform transform)
    {
        AudioSource source = AudioSourcePool.GetAudioSource(transform);

        if (source == null)
        {
            Debug.Log("Source is invalid");
            return;
        }

        source.pitch = 1f;
        source.volume = 1f;
        source.clip = clip;

        source.Play();
    }   
}

[CreateAssetMenu(fileName = "CompositeAudioEmitter", menuName = "Audio/Composite Emitter", order = 2)]
public class CompositeAudioEmitter : AudioEmitter
{
    [SerializeField]
    private AudioClip[] clips;

    public override void Play(Transform transform)
    {
        AudioSource source = AudioSourcePool.GetAudioSource(transform);

        if (source == null)
        {
            Debug.Log("Source is invalid");
            return;
        }

        source.clip = clips[Random.Range(0, clips.Length - 1)];
        source.Play();
    }
}