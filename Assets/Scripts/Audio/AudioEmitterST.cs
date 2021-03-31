using UnityEngine;
using System.Collections.Generic;

/// <summary> 
///     <para> An audio emitter specialized for sound tracks. </para>
///     <para> 
///         <br> * Has all the methods required to make an adaptive sound track. </br>
///     </para>
/// </summary>
public class AudioEventSTInstance
/*  TODO:
 *      Add parameter support.
 *      Add fadeIn/Out/To.
 *      General optimizations.
 */
{
    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.EventDescription description;

    private int length;

    public AudioEventSTInstance(FMOD.Studio.EventInstance instance)
    {
        this.instance = instance;
        instance.getDescription(out description);

        description.getLength(out length);
    }

    public void Start()
    {
        instance.start();
    }

    public void Stop(FMOD.Studio.STOP_MODE mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
    {
        instance.stop(mode);
        instance.setPaused(false);
    }

    public FMOD.Studio.PLAYBACK_STATE GetPlaybackState()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state;
    }

    public int GetLength()
    {
        return length;
    }

    // toggle pause
    public void Pause()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);

        if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            instance.start();
        else if (IsPaused())
            instance.setPaused(false);
        else
            instance.setPaused(true);
    }

    public void SetPaused(bool paused)
    {
        instance.setPaused(paused);
    }

    public bool IsPaused()
    {
        bool isPaused;
        instance.getPaused(out isPaused);
        return isPaused;
    }

    public void SetPitch(float pitch)
    {
        instance.setPitch(pitch);
    }

    public float GetPitch()
    {
        float pitch;
        instance.getPitch(out pitch);
        return pitch;
    }

    public void SetTimelinePosition(int position)
    {
        instance.setTimelinePosition(position);
    }

    public int GetTimelinePosition()
    {
        int position;
        instance.getTimelinePosition(out position);
        return position;
    }

    public void SetTimelinePositionNormalized(float position)
    {
        instance.setTimelinePosition((int)(length / position));
    }

    public float GetTimelinePositionNormalized()
    {
        return length / (float)GetTimelinePosition();
    }

    public void SetVolume(float volume)
    {
        instance.setVolume(volume);
    }

    public float GetVolume()
    {
        float volume;
        instance.getVolume(out volume);
        return volume;
    }
}

public class AudioEmitterST : MonoBehaviour
{
    [SerializeField]
    private AudioEmitterData data;

    private Dictionary<string, AudioEventSTInstance> events = new Dictionary<string, AudioEventSTInstance>();

    private void Awake()
    {
        for (int i = 0; i < data.events.Length; i++)
            events[data.events[i].Substring(data.events[i].LastIndexOf("/") + 1)] = new AudioEventSTInstance(FMODUnity.RuntimeManager.CreateInstance(data.events[i]));
    }

    public AudioEventSTInstance this[string name]
    {
        get { return events[name]; }
    }
}