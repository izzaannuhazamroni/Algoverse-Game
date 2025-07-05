using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SessionData : MonoBehaviour
{
    public static SessionData Instance { get; private set; }
    public string lastSpawnPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

        }
        else
        {
            Instance = this;
            if (transform.parent != null)
            {
                Debug.LogWarning($"[SessionData] ⛓ Terhubung ke parent '{transform.parent.name}', melepas ke root.");
                transform.parent = null;
            }
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string spawnName = SessionData.Instance.lastSpawnPoint;
        if (!string.IsNullOrEmpty(spawnName))
        {
            GameObject spawnPoint = GameObject.Find(spawnName);
            Debug.Log($"[SceneLoaded] Current scene: '{scene.name}'");

            if (spawnPoint != null)
            {
                Debug.Log($"[SceneLoaded] Target spawn point: '{spawnPoint.name}'");

                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    // player.transform.position = spawnPoint.transform.position;
                    Debug.Log($"✅ Berhasil memindahkan player ke '{spawnPoint.name}'");
                }
                else
                {
                    Debug.LogWarning("⚠️ Player dengan tag 'Player' tidak ditemukan di scene.");
                }
            }
            else
            {
                Debug.LogWarning($"⚠️ Spawn point dengan nama '{spawnName}' '{spawnPoint.name}' tidak ditemukan di scene.");
            }
        }

    }

}
