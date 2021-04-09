﻿using UnityEngine;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
 *      Add global parameter support.
 */
{
    private List<FMOD.Studio.EventInstance> instances = new List<FMOD.Studio.EventInstance>();
    private Stack<FMOD.Studio.EventInstance> stoppedInstances = new Stack<FMOD.Studio.EventInstance>();

    private Transform transform = null;
    private string path = "";

    public AudioEventFXInstance(Transform transform, string path)
    {
        this.transform = transform;
        this.path = path;

        AddInstance();
    }

    ~AudioEventFXInstance()
    {
        foreach (FMOD.Studio.EventInstance instance in instances)
            instance.release();
    }

    private FMOD.Studio.EventInstance AddInstance()
    {
        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(path);
        instance.setCallback(OnEventInstanceStopped, FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED);
        instance.setUserData(stoppedInstances.ToIntPtr());

        instances.Add(instance);
        return instance;
    }

    private FMOD.RESULT OnEventInstanceStopped(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr _event, IntPtr parameters)
    {
        test();
        return FMOD.RESULT.OK;
    }

    public void test()
    {
        Debug.Log(path);
    }

    public void Start()
    {
        FMOD.Studio.EventInstance instance;

        if (stoppedInstances.Count == 0)
            instance = AddInstance();
        else
            instance = stoppedInstances.Pop();

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, transform, (Rigidbody2D)null);
        instance.start();
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