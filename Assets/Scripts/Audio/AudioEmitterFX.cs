using UnityEngine;
using System.Collections.Generic;

/// <summary> 
///     <para> An audio emitter specialized for sound effects. </para>
///     <para> 
///         <br> * Can play multiple instances of the same sfx. </br>
///         <br> * Attaches to the game object.                 </br>
///     </para>
/// </summary>....
public class AudioEventFXInstance
/*  TODO:
 *      Optimize the searching process to find a free instance.
 *      Release instances that are not in use for too long.
 */
{
    private List<FMOD.Studio.EventInstance> instances = new List<FMOD.Studio.EventInstance>();

    private Transform transform;
    private string path;

    public AudioEventFXInstance(Transform transform, string path)
    {
        this.transform = transform;
        this.path = path;

        instances.Add(FMODUnity.RuntimeManager.CreateInstance(path));
    }

    ~AudioEventFXInstance()
    {
        foreach (FMOD.Studio.EventInstance instance in instances)
            instance.release();
    }

    public void Start()
    {
        foreach (FMOD.Studio.EventInstance instance in instances)
        {
            FMOD.Studio.PLAYBACK_STATE state;
            instance.getPlaybackState(out state);

            if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, transform, (Rigidbody2D)null);
                instance.start();
                return;
            }
        }

        instances.Add(FMODUnity.RuntimeManager.CreateInstance(path));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instances[instances.Count - 1], transform, (Rigidbody2D)null);
        instances[instances.Count - 1].start();
    }
    
}

public class AudioEmitterFX : MonoBehaviour
{
    [SerializeField] 
    private AudioEmitterData data;

    private Dictionary<string, AudioEventFXInstance> events = new Dictionary<string, AudioEventFXInstance>();

    private void Awake()
    {
        for (int i = 0; i < data.events.Length; i++)
            events[data.events[i].Substring(data.events[i].LastIndexOf("/") + 1)] = new AudioEventFXInstance(transform, data.events[i]);
    }

    public AudioEventFXInstance this[string name]
    {
        get { return events[name]; }
    }
}