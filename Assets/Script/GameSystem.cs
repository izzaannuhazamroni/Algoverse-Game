using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DataGame
{
    public static int DataScore, DataTimes, DataHealth;
}
public class GameSystem : MonoBehaviour
{
    public GameObject CanvasUtama;
    public GameObject CanvasEnd;
    public GameObject CanvasTryAgain;
    public static GameSystem instance;
    public static bool NewGame = true;
    float s;

    [Header("Data Permainan")]
    public bool GameAktif;
    public bool GameDone;
    public string placementTag = "Placement";
    public int Target,DataC;
   

    [Header("Komponen Canvas")]
    public Text TeksSkor;
    public Text TeksWaktu;
    public RectTransform UIHealth;

    private void Awake()
    {
        instance = this;
        SetupCursorForUI();
    }
    void Start()
    {
        GameAktif =false;
        GameDone=false;
        ResetData();
        GameObject[] allPlacements = GameObject.FindGameObjectsWithTag(placementTag);
        Target = allPlacements.Length;
        Debug.Log("Ditemukan " + Target + " objek dengan tag: " + placementTag);
        DataC = 0;
        GameAktif = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameAktif && !GameDone)
        {
            if (DataGame.DataTimes > 0)
            {

                s += Time.deltaTime;
                if( s >= 1)
                {
                    if(DataGame.DataHealth <= 0)
                    {
                        DataGame.DataTimes -= 2;
                    }
                    else
                    {
                        DataGame.DataTimes--;
                    }
                    s = 0;
                }
            }
            if (DataGame.DataTimes <= 0 || DataGame.DataHealth <= 0)
            {
                GameOver();
            }
           
            if (DataC >= Target)
            {
                GameDone = true;
                GameAktif = false;

                StartCoroutine(WinSequence());
            }
        }
        SetInfoUI();
    }
    public void SetInfoUI()
    {
        TeksSkor.text = DataGame.DataScore.ToString();
        //pidnah ke tempat yg udh dibikin atsila soalnya ini masi kebaca terus terusan.
        GameDataManager.Instance.SetMading1Score(DataGame.DataScore);

        int Menit = Mathf.FloorToInt(DataGame.DataTimes / 60);
        int Detik = Mathf.FloorToInt(DataGame.DataTimes % 60);
        TeksWaktu.text = Menit.ToString("00") + " " + Detik.ToString("00");
        UIHealth.sizeDelta = new Vector2(100f * DataGame.DataHealth, 100f);
    }
    void GameOver()
    {
        GameAktif = false;
        GameDone = true;

        Time.timeScale = 0f;

        // 3. Aktifkan/tampilkan "TryAgainCanvas"
        if (CanvasTryAgain != null)
        {
            CanvasUtama.SetActive(false);
            CanvasTryAgain.SetActive(true);
            StartCoroutine(GameOverSequence());
        }
        else
        {
            Debug.LogError("TryAgainCanvas belum di-assign di Inspector!");
        }
    }
    public void ReloadScene()
    {
        ResetData();
        Time.timeScale = 1f;
        // Memuat ulang scene yang sedang aktif saat ini
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Memuat Ulang");
    }

    private IEnumerator WinSequence()
    {
        // 1. Show the end canvas
        if (CanvasEnd != null)
        {
            CanvasEnd.SetActive(true);
        }
        else
        {
            Debug.LogError("End Canvas is not assigned in the Inspector!");
        }

        yield return new WaitForSeconds(5.0f);

        Debug.Log("10 seconds have passed. Loading Lab1...");

        SceneManager.LoadScene("Lab1");
    }
    private void ResetData()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        switch (currentSceneName)
        {
            case "MadingRev":
                DataGame.DataTimes = 60 * 2;
                break;

            case "MadingPrimitif":
                DataGame.DataTimes = 60 * 3;
                break;
        }

        DataGame.DataScore = 0;
        DataGame.DataHealth = 5;
    }
    private void SetupCursorForUI()
    {
        // 1. Membuat kursor menjadi terlihat.
        Cursor.visible = true;

        // 2. Memastikan kursor tidak terkunci di tengah layar.
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Pengaturan kursor untuk scene Mading telah diterapkan: Terlihat & Tidak Terkunci.");
    }
    private IEnumerator GameOverSequence()
    {
        // 1. Sembunyikan canvas utama dan tampilkan canvas 'coba lagi'
        if (CanvasUtama != null)
        {
            CanvasUtama.SetActive(false);
        }
        if (CanvasTryAgain != null)
        {
            CanvasTryAgain.SetActive(true);
        }
        else
        {
            Debug.LogError("CanvasTryAgain belum di-assign!");
            yield break;
        }

        // 2. "Bekukan" game jika Anda mau. Ini opsional tapi disarankan
        Time.timeScale = 0f;

        // 3. Tunggu selama 5 detik WAKTU NYATA (bukan waktu game)
        yield return new WaitForSecondsRealtime(5.0f);

        // 4. Panggil fungsi untuk me-reload scene
        Debug.Log("5 detik telah berlalu. Me-reload scene...");
        ReloadScene();
    }
}
