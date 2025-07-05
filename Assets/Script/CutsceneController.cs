using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        // Deteksi jika pemain menekan tombol Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipCutscene();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void SkipCutscene()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop(); // Hentikan video
            LoadNextScene();    // Pindah ke scene berikutnya
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Maps");
    }
}
