using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioEmitterFX emitter;

    [SerializeField]
    private Slider pitchSlider, globalSlider;

    private void Start()
    {
        pitchSlider.value = emitter["sfx2"].GetParameter("Pitch");
        globalSlider.value = emitter["sfx2"].GetParameter("Global");
    }

    public void OnButtons(string btn)
    {
        // 'btn' can be 1, 2 or 3
        emitter["sfx" + btn].Start();
    }

    public void OnPitchSlider(float value)
    {
        emitter["sfx2"].SetParameter("Pitch", value);
    }

    public void OnGlobalSlider(float value)
    {
        emitter["sfx2"].SetGlobalParameter("Global", value);
    }

}
