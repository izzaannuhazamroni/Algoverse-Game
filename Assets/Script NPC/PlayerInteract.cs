// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerInteract : MonoBehaviour
// {
// [SerializeField] private GameObject pressEUI; // UI "Tekan E"
// private NPCInteractable nearbyNPC;

// private void Update()
// {
//     float interactRange = 2f;
//     nearbyNPC = null;

//     Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
//     foreach (Collider collider in colliderArray)
//     {
//         if (collider.TryGetComponent(out NPCInteractable npcInteractable))
//         {
//             nearbyNPC = npcInteractable;
//             break;
//         }
//     }

//     // Tampilkan atau sembunyikan UI "Tekan E"
//     pressEUI.SetActive(nearbyNPC != null);

//     if (Input.GetKeyDown(KeyCode.E) && nearbyNPC != null)
//     {
//         nearbyNPC.Interact();
//     }
// }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameObject pressEUI;
    private NPCInteractable nearbyNPC;

    private void Start()
{
    if (pressEUI == null)
    {
        // Coba cari berdasarkan nama
        pressEUI = GameObject.Find("PressEUI");

        // Atau bisa juga gunakan tag (jika sudah diberi tag)
        // pressEUI = GameObject.FindWithTag("PressEUI");

        if (pressEUI != null)
        {
            pressEUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("PressEUI tidak ditemukan! Pastikan namanya cocok atau diberi tag khusus.");
        }
    }
}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Masuk trigger dengan: " + other.name); // Tambahkan log ini
        if (other.TryGetComponent(out NPCInteractable npcInteractable))
        {
            nearbyNPC = npcInteractable;
            pressEUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out NPCInteractable npcInteractable))
        {
            if (npcInteractable == nearbyNPC)
            {
                nearbyNPC = null;
                pressEUI.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearbyNPC != null)
        {
            nearbyNPC.Interact();
        }
    }
}