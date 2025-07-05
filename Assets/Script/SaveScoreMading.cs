using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class SaveScoreMading
{
    // Variabel untuk menyimpan skor dari setiap mading.
    public static int scoreMading1 = 0;
    public static int scoreMading2 = 0;

    // Fungsi untuk me-reset semua data saat memulai game baru. Ini sangat penting!
    public static void ResetAllScores()
    {
        scoreMading1 = 0;
        scoreMading2 = 0;

    }
}