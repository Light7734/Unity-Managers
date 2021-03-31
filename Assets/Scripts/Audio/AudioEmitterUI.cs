using UnityEngine;
using System.Collections.Generic;

/// <summary> 
///     <para> An audio emitter specialized for user interface. </para>
/// 
///     <para>
///          <br> * Only has a start method for playing sounds. </br>
///          <br> * Events cannot be interacted with.           </br>
///     </para>
/// </summary>
public class AudioEventUIInstance
{
    private string path;

    public AudioEventUIInstance(string path)
    {
        this.path = path;
    }

    public void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot(path);
    }
}

public class AudioEmitterUI : MonoBehaviour
{
    [SerializeField]
    private AudioEmitterData data;

    private Dictionary<string, AudioEventUIInstance> events = new Dictionary<string, AudioEventUIInstance>();
  
    private void Awake()
    {
        for (int i = 0; i < data.events.Length; i++)
            events[data.events[i].Substring(data.events[i].LastIndexOf("/") + 1)] = new AudioEventUIInstance(data.events[i]);
    }

    public AudioEventUIInstance this[string name]
    {
        get { return events[name]; }
    }
}