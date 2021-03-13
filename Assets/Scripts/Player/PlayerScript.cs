using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public BasicAudioEmitter sfx1;
    public BasicAudioEmitter sfx2;
    public CompositeAudioEmitter sfx3;

    public BasicAudioEmitterST piano;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            SceneManager.LoadNextScene();
    }

    public void OnMusicButtons(string btn)
    {
        if (btn == "play")
            piano.Play();

        if (btn == "pause")
            piano.Pause(true);

        if (btn == "stop")
            piano.Stop();
    }

    public void OnSFXButtons(string btn)
    {
        if (btn == "1")
            sfx1.Play(transform);
        if (btn == "2")
            sfx2.Play(transform);
        if (btn == "3")
            sfx3.Play(transform);
    }

    private void OnDestroy()
    {
        AudioSourcePool.RecollectChildren(transform);
    }
}