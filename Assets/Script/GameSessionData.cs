using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSessionData
{
    public string lastSpawnPoint = "SPFirstPlace";
    public int mading1_correct = 0;
    public int mading2_correct = 0;
    public int final_total_correct = 0;
    public List<int> final_answers = new List<int>();

    public bool IsReadyToUpload()
    {
        return mading1_correct != 0 && mading2_correct != 0 && final_total_correct != 0;
    }
}

[System.Serializable]
public class Record
{
    public int id;
    public string record_time;
    public int mading1_correct;
    public int mading2_correct;
    public int final_total_correct;
    public List<int> final_answers;
}

[System.Serializable]
public class RecordListWrapper
{
    public List<Record> records;
}