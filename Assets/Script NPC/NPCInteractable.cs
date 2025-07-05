using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    [Header("NPC Info")]
    [SerializeField] private string npcName = "Hassan";

    [TextArea(3, 10)]
    [SerializeField] private string[] lines = {
        "Halo, Aqil! Selamat datang di ALGOVERSE.",
        "Di dunia ini, kamu akan belajar algoritma sambil berpetualang!",
        "Sekarang, Mundur dan Masuki Lab Programming untuk memulai petualangan!"
    };

    [Header("UI Prompt")]
    [SerializeField] private GameObject pressEUI;

    private bool isPlayerNearby = false;

    private void Start()
    {
        if (pressEUI == null)
        {
            pressEUI = GameObject.Find("PressEUI");
            Debug.Log("[NPC] 🔍 Mencari PressEUI secara otomatis.");
        }

        if (pressEUI != null)
        {
            pressEUI.SetActive(false);
            Debug.Log("[NPC] ✅ PressEUI ditemukan dan disembunyikan.");
        }
        else
        {
            Debug.LogWarning("[NPC] ❌ PressEUI tidak ditemukan. Pastikan diberi nama atau drag manual.");
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"[NPC] 🗨️ Tombol [E] ditekan oleh Player di dekat '{npcName}'");
            Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (pressEUI != null) pressEUI.SetActive(true);
            Debug.Log($"[NPC] 👋 Player masuk trigger '{npcName}'");
        }
        else
        {
            Debug.Log($"[NPC] ⚠️ Objek lain masuk trigger: {other.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (pressEUI != null) pressEUI.SetActive(false);
            Debug.Log($"[NPC] 🚶 Player keluar dari trigger '{npcName}'");
        }
    }

    public void Interact()
    {
        Debug.Log($"[NPC] 💬 Interaksi dimulai dengan NPC: {npcName}");

        UIChatBubbleManager bubbleManager = UIChatBubbleManager.Instance;
        if (bubbleManager != null)
        {
            bubbleManager.ShowBubble(transform, lines);
            Debug.Log("[NPC] ✅ Chat bubble ditampilkan.");
        }
        else
        {
            Debug.LogWarning("[NPC] ⚠️ UIChatBubbleManager tidak ditemukan!");
        }
    }
}