using UnityEngine;
using UnityEngine.Video;

public class PreMainMenuHandler : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    private bool done = false; // to make sure we don't load level immediate multiple

    void Start()
    {
        videoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!done && videoPlayer.frame >= (long)videoPlayer.frameCount - 1)
        {
            done = true;
            GameManager.LoadLevelImmidiate(GameLevelIndex.MAIN_MENU);
        }
    }
}
