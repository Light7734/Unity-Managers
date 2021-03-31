using UnityEngine;

[CreateAssetMenu(fileName = "AudioEmitterData", menuName = "Audio/EmitterData")]
public class AudioEmitterData : ScriptableObject
{
    [FMODUnity.EventRef]
    public string[] events;
}