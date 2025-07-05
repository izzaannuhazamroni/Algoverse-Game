using UnityEngine;
using UnityEngine.SceneManagement; // Jika Anda punya tombol untuk kembali ke menu

public class PauseManager : MonoBehaviour
{
    // Variabel publik untuk dihubungkan di Inspector
    public GameObject pauseMenuUI;

    // Variabel untuk melacak status pause game
    public static bool isGamePaused = false;

    // Singleton pattern (opsional, tapi berguna)
    public static PauseManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    void Start()
    {
        // Pastikan menu pause disembunyikan di awal
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        isGamePaused = false;
        // Pastikan waktu berjalan normal di awal
        Time.timeScale = 1f;
    }

    // Update dipanggil setiap frame
    void Update()
    {
        // Mendeteksi apakah tombol "Escape" ditekan
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                // Jika game sudah di-pause, maka kita Resume
                ResumeGame();
            }
            else
            {
                // Jika game sedang berjalan, maka kita Pause
                PauseGame();
            }
        }
    }

    // --- Fungsi Publik untuk Tombol dan Logika ---

    public void ResumeGame()
    {
        // Sembunyikan UI menu pause
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        // Update status game
        isGamePaused = false;

        // Tampilkan kembali kursor untuk gameplay 3D jika perlu
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;

        Debug.Log("Game Resumed");
    }

    void PauseGame()
    {
        // Tampilkan UI menu pause
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }


            // Time.timeScale = 0f; // << INI HARUS ADA

        // Update status game
        isGamePaused = true;

        // Bebaskan kursor agar pemain bisa mengklik tombol di menu pause
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Game Paused");
    }

    public void QuitToMainMenu()
    {
        // Pastikan waktu berjalan normal sebelum pindah scene
        Time.timeScale = 1f;
        isGamePaused = false;

        SceneManager.LoadScene("MainMenu"); // Ganti "MainMenu" dengan nama scene menu utama Anda
    }
    public void QuitToDesktop()
    {
        // Pastikan waktu berjalan normal sebelum pindah scene
        Time.timeScale = 1f;
        isGamePaused = false;

        Application.Quit();
    }
}