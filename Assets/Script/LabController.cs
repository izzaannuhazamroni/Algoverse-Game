using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LabController : MonoBehaviour
{
    public Image requirementImageHolder;
    public Sprite successSprite;
    public Sprite failedSprite;
    public Text allScore;

    void Start()
    {
        CheckFinalTestRequirement();
    }

    public void CheckFinalTestRequirement()
    {
        // Ambil skor dari papan tulis global kita
        int score1 = SaveScoreMading.scoreMading1;
        int score2 = SaveScoreMading.scoreMading2;
        int sc12 = score1 + score2;

        Debug.Log("Pengecekan di Lab: Skor Mading 1=" + score1 + ", Skor Mading 2=" + score2);

        bool isRequirementMet = (score1 >= 20 && score2 >= 60);
        allScore.text = sc12.ToString();
        // Beri feedback visual kepada pemain
        if (requirementImageHolder != null)
        {
            requirementImageHolder.enabled = true;
            if (isRequirementMet)
            {
                requirementImageHolder.sprite = successSprite;
                 requirementImageHolder.color = Color.green;
            }
            else
            {
                requirementImageHolder.sprite = failedSprite;
                 requirementImageHolder.color = Color.red;
            }
        }
        else
        {
            Debug.LogError("Requirement Image Holder belum di-assign di Inspector!");
        }
    }

}
