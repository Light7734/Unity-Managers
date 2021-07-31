using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary> 
///     <para> An audio emitter specialized for sound effects. </para>
///     <para> 
///         <br> * Can play multiple instances of the same sfx. </br>
///         <br> * Attaches to the game object.                 </br>
///     </para>
/// </summary>....
public class AudioEventFXInstance
/*  TODO:
 *      Release instances that are not in use for too long.
 *      Add parameter support.
 */
{
    public class Parameter
    {
        public Parameter(FMOD.Studio.PARAMETER_ID id, float value)
        {
            this.id = id;
            this.value = value;
        }

        public FMOD.Studio.PARAMETER_ID id;
        public float value;
    }

    private List<FMOD.Studio.EventInstance> instances = new List<FMOD.Studio.EventInstance>();
    private Stack<FMOD.Studio.EventInstance> stoppedInstances = new Stack<FMOD.Studio.EventInstance>();
    private IntPtr stackPtr;

    private Dictionary<string, Parameter> parameters = new Dictionary<string, Parameter>();

    private Transform transform = null;
    private string path = "";

    FMOD.Studio.EVENT_CALLBACK callback;

    public AudioEventFXInstance(Transform transform, string path)
    {
        this.transform = transform;
        this.path = path;
        this.callback = OnEventInstanceStopped;

        stackPtr = GCHandle.ToIntPtr(GCHandle.Alloc(stoppedInstances));
        AddInstance();

        FMOD.Studio.EventDescription description;
        int count;

        instances[0].getDescription(out description);
        description.getParameterDescriptionCount(out count);

        for (int i = 0; i < count; i++)
        {
            FMOD.Studio.PARAMETER_DESCRIPTION paramDescription;
            description.getParameterDescriptionByIndex(i, out paramDescription);
            parameters.Add(paramDescription.name, new Parameter(paramDescription.id, paramDescription.defaultvalue));
            Debug.Log(paramDescription.name);
        }
    }

    ~AudioEventFXInstance()
    {
        foreach (FMOD.Studio.EventInstance instance in instances)
            instance.release();
    }

    private void AddInstance()
    {
        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(path);
        instance.setCallback(callback, FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED);
        instance.setUserData(stackPtr);

        foreach (KeyValuePair<string, Parameter> parameter in parameters)
            instance.setParameterByID(parameter.Value.id, parameter.Value.value);

        instances.Add(instance);
        stoppedInstances.Push(instance);
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static private FMOD.RESULT OnEventInstanceStopped(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr _event, IntPtr parameters)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(_event);

        IntPtr data;
        instance.getUserData(out data);

        ((Stack<FMOD.Studio.EventInstance>)GCHandle.FromIntPtr(data).Target).Push(instance);

        return FMOD.RESULT.OK;
    }

    public void Start()
    {
        if (stoppedInstances.Count == 0)
            AddInstance();

        FMOD.Studio.EventInstance instance = stoppedInstances.Pop();

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, transform, (Rigidbody2D)null);
        instance.start();
    }

    public void SetParameter(string name, float value)
    {
        if (!parameters.ContainsKey(name))
        {
            Debug.LogError("[AudioEmitterFX.SetParameter]: failed to find parameter with name \"" + name + '\"');
            return;
        }

        parameters[name].value = value;

        foreach (FMOD.Studio.EventInstance instance in instances)
            instance.setParameterByID(parameters[name].id, value);
    }

    public void SetGlobalParameter(string name, float value)
    {
        if (!parameters.ContainsKey(name))
        {
            Debug.LogError("[AudioEmitterFX.SetGlobalParameter]: failed to find parameter with name \"" + name + '\"');
            return;
        }

        FMODUnity.RuntimeManager.StudioSystem.setParameterByID(parameters[name].id, value);
    }

    public float GetParameter(string name)
    {
        if (!parameters.ContainsKey(name))
        {
            Debug.LogError("[AudioEmitterFX.GetParameter]: failed to find parameter with name \"" + name + '\"');
            return 0f;
        }

        return parameters[name].value;
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