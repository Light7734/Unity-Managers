using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public AudioPlayer audioPlayer;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
            audioPlayer.Play<AudioEmitter>("crystal", transform);
        if (Input.GetKey(KeyCode.B))
            SceneManager.LoadNextScene();
    }

    private void OnDestroy()
    {
        AudioSourcePool.RecollectChildren(transform);
    }
}