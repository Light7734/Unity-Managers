using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public AudioEmitterFX sfxEmitter;
    public AudioEmitterST musicEmitter;

    public Slider slider;

    private void Start()
    {
        slider.minValue = 0f;
        slider.maxValue = musicEmitter["music"].GetLength();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            SceneManager.LoadNextScene();

        slider.value = musicEmitter["music"].GetTimelinePosition();
    }

    public void OnPlayButton()
    {
        musicEmitter["music"].Pause();
    }

    public void OnSFXButtons(string btn)
    {
        if (btn == "1")
            sfxEmitter["sfx1"].Start();
        if (btn == "2")
            sfxEmitter["sfx2"].Start();
        if (btn == "3")
            sfxEmitter["sfx3"].Start();
    }

    public void OnSlider(float v)
    {
        if ((int)v != musicEmitter["music"].GetTimelinePosition())
            musicEmitter["music"].SetTimelinePosition((int)v);
    }

    public void OnSadnessSlider(float v)
    {
        musicEmitter["music"].SetParameter("Sadnes", v);
    }


}