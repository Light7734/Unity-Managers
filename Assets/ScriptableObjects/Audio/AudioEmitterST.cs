using UnityEngine;

public abstract class AudioEmitterST : ScriptableObject
{
    public abstract void Play();
}

[CreateAssetMenu(fileName = "BasicAudioEmitterST", menuName = "AudioST/Basic EmitterST", order = 1)]
public class BasicAudioEmitterST : AudioEmitterST
{
    [SerializeField]
    private AudioClip clip;

    private AudioSource audioSource;

    public override void Play()
    {
        if (audioSource == null)
        {
            audioSource = AudioSourcePool.GetAudioSourceST();
            if (audioSource == null)
            {
                Debug.Log("Source is invalid");
                return;
            }

            audioSource.loop = true;
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void Stop()
    {
        if (audioSource == null)
            return;

        audioSource.Stop();
    }

    public void Pause(bool toggle)
    {
        if (audioSource == null)
            return;

        if (audioSource.isPlaying)
            audioSource.Pause();
        else if (toggle)
            audioSource.UnPause();
    }

    public void UnPause()
    {
        if (audioSource == null)
            return;

        audioSource.UnPause();
    }

}