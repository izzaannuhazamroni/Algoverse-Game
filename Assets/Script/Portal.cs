using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Scene dan Spawn Info")]
    public string sceneTujuan;
    public string spawnPointNama;

    [Header("UI Petunjuk (Hint)")]
    public GameObject panelHintObject;

    private void Start()
    {
        if (panelHintObject != null)
        {
            panelHintObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
               if (other.CompareTag("Player"))
        {
            // 2. Jika itu adalah Player, kita aktifkan/tampilkan panel petunjuk.
            if (panelHintObject != null)
            {
                panelHintObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 1. Kita periksa lagi apakah itu adalah "Player" yang keluar dari zona ini.
        if (other.CompareTag("Player"))
        {
            // 2. Jika ya, kita nonaktifkan/sembunyikan lagi panel petunjuknya.
            if (panelHintObject != null)
            {
                panelHintObject.SetActive(false);
            }
        }
    }
}
