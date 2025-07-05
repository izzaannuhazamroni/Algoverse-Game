using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MadingEnd : MonoBehaviour
{
    public Text TextSkor;
    //public Text TextTotalSkor;
    private int currentMadingScore = 0;
    void Start()
    {
        // 1. Simpan skor akhir dari mading ini ke variabel lokal.
        currentMadingScore = DataGame.DataScore;

        // 2. Tampilkan skor saat ini ke UI.
        TextSkor.text = currentMadingScore.ToString();

        // 3. Panggil fungsi untuk menyimpan skor ini secara persistens.
        SaveMadingScore();
    }

    private void SaveMadingScore()
    {
        // 4. Dapatkan nama scene yang sedang aktif.
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 5. Gunakan switch case untuk menentukan ke mana skor akan disimpan.
        switch (currentSceneName)
        {
            case "MadingRev":
                SaveScoreMading.scoreMading1 = currentMadingScore;
                GameDataManager.Instance.SetMading1Score(currentMadingScore);
                Debug.Log("Skor untuk Mading Referensi (" + currentMadingScore + ") telah disimpan ke DataGame.");
                break; 

            case "MadingPrimitif":
                SaveScoreMading.scoreMading2 = currentMadingScore;
                GameDataManager.Instance.SetMading2Score(currentMadingScore);
                Debug.Log("Skor untuk Mading Primitif (" + currentMadingScore + ") telah disimpan ke DataGame.");
                break;


            default:
                Debug.LogWarning("Scene '" + currentSceneName + "' tidak memiliki pengaturan penyimpanan skor di MadingEnd.cs. Skor tidak disimpan secara persistens.");
                break;
        }
    }
}
