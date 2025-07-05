using System.Collections;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    public GameObject uiObject;
    private Coroutine hideCoroutine;

    void Start()
    {
        if (uiObject != null)
        {
            uiObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("uiObject belum di-assign di inspector!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Selalu periksa tag untuk memastikan itu adalah Player.
        if (other.CompareTag("Player"))
        {
            // Jika ada coroutine penyembunyian yang sedang berjalan, hentikan.
            // Ini mencegah UI hilang jika pemain keluar lalu masuk lagi dengan cepat.
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
                hideCoroutine = null;
            }

            // Tampilkan UI.
            if (uiObject != null)
            {
                uiObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Periksa lagi apakah itu Player yang keluar.
        if (other.CompareTag("Player"))
        {
            // Sembunyikan UI.
            if (uiObject != null)
            {
                uiObject.SetActive(false);
            }
        }
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(7f);

        if (uiObject != null)
        {
            Destroy(uiObject);
        }

        Destroy(gameObject);
    }
}
