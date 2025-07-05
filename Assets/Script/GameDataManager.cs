using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public GameSessionData sessionData = new GameSessionData();
    public List<Record> fetchedRecords = null;
    [Header("Prefab")]
    public GameObject playerPrefab;  // Drag prefab PlayerArmature ke sini di Inspector


    private void Awake()
    {
        // 🔍 Cek posisi di hierarchy
        // string fullHierarchy = gameObject.name;
        // Transform current = transform.parent;

        // while (current != null)
        // {
        //     fullHierarchy = current.name + "/" + fullHierarchy;
        //     current = current.parent;
        // }

        // if (transform.parent == null)
        // {
        //     Debug.Log("✅ GameDataManager berada di root Hierarchy: " + fullHierarchy);
        // }
        // else
        // {
        //     Debug.LogWarning("❌ GameDataManager adalah child. Hierarchy path: " + fullHierarchy);
        // }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SetMading1Score(int score)
    {
        sessionData.mading1_correct = score;
        Debug.Log("Mading 1 saved: " + score);
        string jsonData = JsonUtility.ToJson(sessionData);
        Debug.Log("Kirim JSON: " + jsonData);
    }

    public void SetMading2Score(int score)
    {
        sessionData.mading2_correct = score;
        Debug.Log("Mading 2 saved: " + score);
        string jsonData = JsonUtility.ToJson(sessionData);
        Debug.Log("Kirim JSON: " + jsonData);
    }

    public void SetFinalScore(int score, List<int> answers)
    {
        sessionData.final_total_correct = score;
        sessionData.final_answers = answers;
        Debug.Log("Final test saved.");
        string jsonData = JsonUtility.ToJson(sessionData);
        Debug.Log("Kirim JSON: " + jsonData);
    }

    public GameSessionData GetFinalData()
    {
        return sessionData;
    }

    public void SubmitFinalData()
    {
        if (sessionData.IsReadyToUpload())
        {
            string jsonData = JsonUtility.ToJson(sessionData);
            Debug.Log("Kirim JSON: " + jsonData);
            StartCoroutine(PostData("http://localhost:3000/api/records", jsonData));
        }
        else
        {
            Debug.LogWarning("Data belum lengkap. Tidak dikirim.");
        }
    }
    IEnumerator PostData(string url, string json)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data berhasil dikirim!");
        }
        else
        {
            Debug.LogError("Gagal kirim data: " + request.error);
        }
    }
    
    public void FetchAllRecords()
{
    StartCoroutine(GetAllRecords());
}

    IEnumerator GetAllRecords()
    {
        string url = "http://localhost:3000/api/records";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("Response dari server:\n" + json);

            RecordListWrapper wrapper = JsonUtility.FromJson<RecordListWrapper>("{\"records\":" + json + "}");
            fetchedRecords = wrapper.records;
        }
        else
        {
            Debug.LogError("Gagal fetch data: " + request.error);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string spawnName = Instance.sessionData.lastSpawnPoint;
        Debug.Log($"[SceneLoaded] Current scene: '{scene.name}'");

        if (!string.IsNullOrEmpty(spawnName))
        {
            GameObject spawnPoint = GameObject.Find(spawnName);

            if (spawnPoint != null)
            {
                Debug.Log($"[SceneLoaded] Spawning PlayerArmature at '{spawnPoint.name}'");

                // Hapus player lama jika ada
                GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
                if (existingPlayer != null)
                {
                    Destroy(existingPlayer);
                    Debug.Log("[SceneLoaded] 🔄 Player lama dihancurkan.");
                }

                // Tentukan posisi spawn
                Vector3 spawnPosition = spawnPoint.transform.position;
                if (scene.name == "Lab1")
                {
                    spawnPosition += new Vector3(0f, 0f, 2.5f); // tambah 3 unit di sumbu Z
                    Debug.Log("[SceneLoaded] 🧭 Lab1 terdeteksi: posisi Z ditambah 3.");
                }

                // Spawn prefab
                GameObject newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
                Debug.Log($"✅ PlayerArmature berhasil di-spawn di '{spawnPoint.name}'");

                // Cari CinemachineVirtualCamera dan sambungkan ke PlayerCameraRoot
                var virtualCam = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
                if (virtualCam != null)
                {
                    Transform cameraRoot = newPlayer.transform.Find("PlayerCameraRoot");
                    if (cameraRoot != null)
                    {
                        virtualCam.Follow = cameraRoot;
                        virtualCam.LookAt = cameraRoot;
                        Debug.Log("[SceneLoaded] 🎥 Kamera diatur follow dan lookAt ke PlayerCameraRoot.");
                    }
                    else
                    {
                        Debug.LogWarning("⚠️ PlayerCameraRoot tidak ditemukan di dalam prefab.");
                    }
                }
                else
                {
                    Debug.LogWarning("⚠️ CinemachineVirtualCamera tidak ditemukan di scene.");
                }
            }
            else
            {
                Debug.LogWarning($"⚠️ Spawn point '{spawnName}' tidak ditemukan.");
            }
        }
        else
        {
            Debug.Log("[SceneLoaded] ❌ Tidak ada nama spawn point tersimpan.");
        }
    }

}
