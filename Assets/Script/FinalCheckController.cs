using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager ini bertanggung jawab untuk menangani event saat scene dimuat.
/// Ia akan tetap ada (persistent) di seluruh scene.
/// </summary>
public class FinalCheckController : MonoBehaviour
{
    // Gunakan Singleton pattern agar mudah diakses dan hanya ada satu instance.
    public static FinalCheckController Instance;

    [Header("Pengaturan untuk Scene 'lab1'")]
    [Tooltip("Seret (Drag) Child GameObject yang memiliki komponen Trigger ke sini. Objek ini akan dinonaktifkan saat 'lab1' dimuat.")]
    public string SPName;
    private GameObject PortalTrigger;

    private void Awake()
    {
        // Setup Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            // Jangan hancurkan objek ini saat memuat scene baru.
            DontDestroyOnLoad(gameObject);
            
        }
        else if (Instance != this)
        {
            // Jika sudah ada instance lain, hancurkan yang ini agar tidak duplikat.
            Destroy(gameObject);
            return; // Hentikan eksekusi lebih lanjut di Awake.
        }
    }

    private void OnEnable()
    {
        // Berlangganan (subscribe) ke event sceneLoaded.
        // Setiap kali scene dimuat, method 'OnSceneLoaded' akan dipanggil.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Penting: Selalu berhenti berlangganan (unsubscribe) saat objek dinonaktifkan atau dihancurkan.
        // Ini untuk mencegah memory leak.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Method ini akan dijalankan secara otomatis setiap kali sebuah scene selesai dimuat.
    /// </summary>
    /// <param name="scene">Informasi scene yang baru dimuat.</param>
    /// <param name="mode">Mode pemuatan scene (Single atau Additive).</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cek apakah nama scene yang dimuat adalah "lab1"
        if (scene.name == "Lab1")
        {
            Debug.Log($"Scene '{scene.name}' telah dimuat. Menjalankan aksi yang diperlukan.");
            // Coba cari PortalTrigger secara otomatis jika belum diset
            if (PortalTrigger == null)
            {
                GameObject found = GameObject.Find(SPName); // Ganti sesuai nama
                if (found != null)
                {
                    Transform child = found.transform.Find("PortalTrigger");
                    PortalTrigger = child.gameObject;
                    Debug.Log($"PortalTrigger ditemukan otomatis: {found.name}");
                }
                else
                {
                    Debug.LogWarning("PortalTrigger tidak ditemukan secara otomatis di scene.");
                }
            }

            // 1. Menonaktifkan objek trigger yang ditentukan
            if (PortalTrigger != null)
            {
                if (GameDataManager.Instance != null)
                {
                    if (!GameDataManager.Instance.sessionData.IsReadyToUpload())
                    {
                        PortalTrigger.SetActive(false);
                        Debug.Log($"Objek '{PortalTrigger.name}' telah dinonaktifkan.");
                    }
                    else
                    {
                        PortalTrigger.SetActive(true);
                        Debug.Log($"Objek '{PortalTrigger.name}' telah diaktifkan.");
                        // Pastikan Anda memiliki skrip GameDataManager dengan method ini.
                        GameDataManager.Instance.SubmitFinalData();
                        Debug.Log("GameDataManager.SubmitFinalData() telah dipanggil.");
                    }
                }
                else
                {
                    Debug.LogWarning("GameDataManager belum siap.");

                }
            }
            else
            {
                Debug.LogWarning("Referensi 'PortalTrigger' belum diatur di FinalCheckController.");
            }

        }
    }
}