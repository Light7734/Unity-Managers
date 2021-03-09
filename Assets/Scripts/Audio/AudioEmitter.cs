/* TODO:
 * Handle the case where EmmitSoundtrack* gets called while a fading is still in action
 * Handle other special cases
 * Add AudioMixer support
 */
using System.Collections;
using UnityEngine;

/// <summary>
/// Audio emitter that plays sounds through PlayOneShot() function
/// <para> Use for SFX, short sounds and anything that's not music and not looped </para>
/// </summary>
public class AudioEmitter : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    public void EmitSound(AudioName name)
    {
        source.PlayOneShot(AudioManager.i.GetClip(name));
    }
}

/// <summary>
/// Audio emitter specialized for music and ambient tracks (AudioEmitterSoundtrack)
/// </summary>
public class AudioEmitterST : MonoBehaviour
{
    private AudioSource activeSource;
    private AudioSource source1;
    private AudioSource source2;

    private bool crossFading = false;
    private bool fading = false;

    private void Awake()
    {
        // source1
        source1 = gameObject.AddComponent<AudioSource>();
        source1.volume = 0f;
        source1.loop = true;

        // source2
        source2 = gameObject.AddComponent<AudioSource>();
        source2.volume = 0f;
        source2.loop = true;
       
        activeSource = source1;
    }

    /// <summary>
    /// Plays a sound track and instantly mutes the previous ones (if any)
    /// </summary>
    /// <param name="name"> The name of the track </param>
    /// <param name="volume"> The loudness of the track </param>
    /// <param name="fadeIn"> The time it takes (in second) for the track to reaach the volume </param>
    public void EmitSoundtrack(AudioName name, float volume = 1f, float fadeIn = 10f)
    {
        // TODO: Handle collisions
        if (crossFading)
            Debug.LogWarning("[AudioEmitterST]: EmitSoundtrack was called while cross fading still in action");
        if (fading)
            Debug.LogWarning("[AudioEmitterST]: EmitSoundtrack was called while fading still in action");

        // Play
        activeSource.clip = AudioManager.i.GetClip(name);
        activeSource.Play();

        // Fade in
        StartCoroutine(FadeIn(volume, fadeIn));
    }

    /// <summary>
    /// Plays a sound track by fading between the previous and the new one (if any)
    /// </summary>
    /// <param name="trackName"> The name of the track </param>
    /// <param name="volume"> The loudness of the track </param>
    /// <param name="prevFade"> The time it takes (in seconds) for the previous track to fade out </param>
    /// <param name="newFade"> The time it takes (in seconds) for the new track to reach the volume </param>
    public void EmitSoundtrackFade(AudioName trackName, float volume = 1f, float prevFade = 1f, float newFade = 1f)
    {
        // TODO: Handle collisions
        if (crossFading)
            Debug.LogWarning("[AudioEmitterST]: EmitSoundtrackFade was called while cross fading still in action");
        if (fading)
            Debug.LogWarning("[AudioEmitterST]: EmitSoundtrackFade was called while fading still in action");

        // Play with the new active source
        activeSource = source1.isPlaying ? source2 : source1;
        activeSource.clip = AudioManager.i.GetClip(trackName);
        activeSource.Play();

        // Fade
        StartCoroutine(Fade(activeSource == source1 ? source2 : source1, activeSource, volume, prevFade, newFade));
    }

    /// <summary>
    /// Plays a sound track by cross fading between the previous and the new one (if any)
    /// </summary>
    /// <param name="trackName"> The name of the track </param>
    /// <param name="volume"> The loudness of the track </param>
    /// <param name="prevFade"> The time it takes (in seconds) for the previous track to fade out </param>
    /// <param name="newFade"> The time it takes (in seconds) for the new track to reach the volume </param>
    public void EmitSoundtrackCrossFade(AudioName trackName, float volume = 1f,  float prevFade = 5f, float newFade = 5f)
    {
        // TODO: Handle collisions
        if (crossFading)
            Debug.LogWarning("[AudioEmitterST]: EmitSoundtrackCrossFade was called while cross fading still in action");
        if (fading)
            Debug.LogWarning("[AudioEmitterST]: EmitSoundtrackCrossFade was called while fading still in action");

        // Play with the new active source
        activeSource = source1.isPlaying ? source2 : source1;
        activeSource.clip = AudioManager.i.GetClip(trackName);
        activeSource.Play();

        // Cross fade
        StartCoroutine(CrossFade(activeSource == source1 ? source2 : source1 , activeSource, volume, prevFade, newFade));
    }
    
    private IEnumerator FadeIn(float volume, float fade)
    {
        // Difference between the target volume and the current volume
        float diff = volume - activeSource.volume;

        // Fade in //
        while (activeSource.volume < volume)
        {
            Debug.Log(activeSource.volume);
            if (activeSource.volume < volume)
                activeSource.volume += (Time.deltaTime / fade) * diff;

            yield return null;
        }

        // adjust the volume
        activeSource.volume = volume;
    }

    private IEnumerator Fade(AudioSource prevSource, AudioSource newSource, float volume, float prevFade, float newFade)
    {
        fading = true;

        // Difference between the target volume and the current volume
        float diffPrev = prevSource.volume;
        float diffNew = volume - newSource.volume;

        // Fade out
        while (prevSource.volume > 0f)
        {
            prevSource.volume -= (Time.deltaTime / prevFade) * diffPrev;
            yield return null;
        }

        // adjust the volume
        prevSource.volume = 0f;
        prevSource.Stop();

        // Fade in
        while (newSource.volume < volume)
        {
            newSource.volume += (Time.deltaTime / newFade) * diffNew;
            yield return null;
        }

        // adjust the volume
        newSource.volume = volume;

        fading = false;
    }

    private IEnumerator CrossFade(AudioSource prevSource, AudioSource newSource, float volume, float prevFade, float newFade)
    {
        crossFading = true;

        // Difference between the target volume and the current volume
        float diffPrev = prevSource.volume;
        float diffNew = volume - newSource.volume;

        // CrossFade
        while(prevSource.volume > 0f || newSource.volume < volume)
        {
            Debug.Log(prevSource.volume);

            if (newSource.volume < volume)
                newSource.volume += (Time.deltaTime / newFade) * diffNew;

            if(prevSource.volume > 0f)
                prevSource.volume -= (Time.deltaTime / prevFade) * diffPrev;

            yield return null;
        }

        // adjust the volumes
        prevSource.volume = 0f;
        newSource.volume = volume;

        prevSource.Stop();
        crossFading = false;
    }
}

