using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalEnter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject portalPromptUI;
    private Portal portalInRange;

    void Awake()
    {
        // Tambahkan listener agar saat scene berganti, kita cari ulang PressFUI
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (portalPromptUI == null)
        {
            GameObject foundUI = GameObject.FindGameObjectWithTag("PressFUI");
            if (foundUI != null)
            {
                portalPromptUI = foundUI;
                portalPromptUI.SetActive(false);
                Debug.Log("[PortalEnter] 🔎 PressFUI ditemukan di scene baru.");
            }
            else
            {
                Debug.Log("[PortalEnter] ⚠️ Tidak menemukan GameObject dengan tag 'PressFUI'.");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && portalInRange != null)
        {
            string spawnName = portalInRange.spawnPointNama;

            if (string.IsNullOrEmpty(spawnName))
            {
                Debug.LogError("[PortalEnter] ❌ spawnPointNama kosong atau NULL!");
                return;
            }

            GameDataManager.Instance.sessionData.lastSpawnPoint = spawnName;

            Debug.Log($"[PortalEnter] 🔄 Player menekan [F] di portal.");
            Debug.Log($"[PortalEnter] ⛩️ Portal tujuan: {portalInRange.sceneTujuan}");
            Debug.Log($"[PortalEnter] 💾 Menyimpan spawn point: {spawnName}");

            HandlePortalInteraction(portalInRange);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Portal>() is Portal portal)
        {
            portalInRange = portal;

            if (portalPromptUI != null)
            {
                portalPromptUI.SetActive(true);
                Debug.Log($"[PortalEnter] ✅ Masuk area portal: {portal.name}. Prompt UI ditampilkan.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Portal>() == portalInRange)
        {
            portalInRange = null;
            if (portalPromptUI != null)
            {
                portalPromptUI.SetActive(false);
                Debug.Log($"[PortalEnter] ⛔ Keluar area portal. Prompt UI disembunyikan.");
            }
        }
    }

    private void HandlePortalInteraction(Portal portal)
    {
        if (portal == null)
        {
            Debug.LogWarning("[PortalEnter] ⚠️ Portal tidak ditemukan saat interaksi.");
            return;
        }

        Debug.Log($"[PortalEnter] 🔁 Memuat scene tujuan: {portal.sceneTujuan}");
        SceneManager.LoadScene(portal.sceneTujuan);
    }
}
