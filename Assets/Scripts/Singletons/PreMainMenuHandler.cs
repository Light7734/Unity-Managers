using UnityEngine;
using UnityEngine.Video;

public class PreMainMenuHandler : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer.frame >= (long)videoPlayer.frameCount - 1)
            GameManager.LoadLevelImmediate(GameLevelIndex.MAIN_MENU, true);
    }
}
