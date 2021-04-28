using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioEmitterST emitter;

    [SerializeField] private Slider timelineSlider, pitchSlider;

    private void Start()
    {
        timelineSlider.minValue = 0f;
        timelineSlider.maxValue = emitter["music"].GetLength();
        timelineSlider.wholeNumbers = true;

        pitchSlider.value = emitter["music"].GetParameter("Sadness");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            SceneManager.LoadNextScene();

        timelineSlider.value = emitter["music"].GetTimelinePosition();
    }

    public void OnButtons()
    {
        emitter["music"].Pause();
    }

    public void OnTimelineSlider(float value)
    {
        if ((int)value != emitter["music"].GetTimelinePosition())
            emitter["music"].SetTimelinePosition((int)value);
    }

    public void OnPitchSlider(float value)
    {
        emitter["music"].SetParameter("Sadness", value);
    }

}