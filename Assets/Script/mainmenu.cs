using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("CutScene1");
    }

    public void Exit()
    {
        // Berfungsi kalau sudah di-build
        Application.Quit();
        Debug.Log("Exit");

        // Berfungsi saat di Editor Unity (menghentikan Play Mode)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

