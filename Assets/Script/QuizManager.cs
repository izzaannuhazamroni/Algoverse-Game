// QuizManagerWithCF.cs
// Gabungan modul QuizManager + CollaborativeFilteringManager untuk menentukan urutan soal berdasarkan CF

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswer> QnA;
    public GameObject[] options;
    public GameObject Quizpanel;
    public GameObject Resultpanel;

    public Image QuestionImg;
    public TMP_Text ScoreTxt;
    public TMP_Text SeluruhSoalTxt;
    public TMP_Text JumlahBenarTxt;
    public TMP_Text FinalScoreTxt;
    public TMP_Text timerText;

    public float timeRemaining = 120f;
    private bool isQuizActive = true;

    private int totalQuestions = 0;
    public int score = 0;
    public int currentQuestion;
    private List<int> answerLog = new List<int>();

    public int maxSimilarUsers = 4;
    public int maxAllowedDifference = 20;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        totalQuestions = QnA.Count;
        Resultpanel.SetActive(false);
        StartCoroutine(PrepareSoalWithCF());
    }

    IEnumerator PrepareSoalWithCF()
    {
        yield return new WaitUntil(() => GameDataManager.Instance.fetchedRecords != null);

        var orderedIndex = GetRecommendedQuestionOrder(
            GameDataManager.Instance.fetchedRecords,
            GameDataManager.Instance.sessionData.mading1_correct,
            GameDataManager.Instance.sessionData.mading2_correct,
            QnA.Count
        );

        List<QuestionAndAnswer> sortedQnA = new List<QuestionAndAnswer>();
        foreach (var idx in orderedIndex)
        {
            sortedQnA.Add(QnA[idx]);
        }
        QnA = sortedQnA;

        answerLog = new List<int>(new int[QnA.Count]);
        for (int i = 0; i < answerLog.Count; i++) answerLog[i] = -1;

        generateQuestion();
    }

    List<int> GetRecommendedQuestionOrder(List<Record> allRecords, int mading1New, int mading2New, int finalSoalCount)
    {
        Debug.Log("\n=== [CF] Memulai Collaborative Filtering ===");
        Debug.Log($"User baru - Mading1: {mading1New}, Mading2: {mading2New}");
        Debug.Log($"Jumlah data user lama: {allRecords.Count}");

        List<(Record user, int difference)> similarUsers = new List<(Record, int)>();

        foreach (var record in allRecords)
        {
            int diff = Mathf.Abs(record.mading1_correct - mading1New) + Mathf.Abs(record.mading2_correct - mading2New);
            Debug.Log($"Perbandingan dengan User#{record.id} | M1: {record.mading1_correct}, M2: {record.mading2_correct}, Diff: {diff}");
            if (diff <= maxAllowedDifference)
            {
                similarUsers.Add((record, diff));
            }
        }

        if (similarUsers.Count == 0)
        {
            Debug.Log("[CF] Tidak ditemukan user mirip. Menggunakan urutan soal default.");
            return Enumerable.Range(0, finalSoalCount).ToList();
        }

        var topUsers = similarUsers.OrderBy(x => x.difference).Take(maxSimilarUsers).Select(x => x.user).ToList();
        Debug.Log($"[CF] Ditemukan {topUsers.Count} user mirip. Mulai menghitung kesalahan tiap soal.");

        int[] salahCount = new int[finalSoalCount];
        foreach (var user in topUsers)
        {
            Debug.Log($"Evaluasi User#{user.id} final_answers: [{string.Join(",", user.final_answers)}]");
            for (int i = 0; i < finalSoalCount; i++)
            {
                if (user.final_answers[i] == 0)
                {
                    salahCount[i]++;
                }
            }
        }

        for (int i = 0; i < salahCount.Length; i++)
        {
            Debug.Log($"Soal #{i} salah oleh {salahCount[i]} user mirip.");
        }

        List<int> orderedIndices = Enumerable.Range(0, finalSoalCount).ToList();
        orderedIndices.Sort((a, b) => salahCount[b].CompareTo(salahCount[a]));

        Debug.Log("[CF] Urutan soal berdasarkan prediksi kesalahan: [" + string.Join(", ", orderedIndices) + "]\n");

        return orderedIndices;
    }

    private void Update()
    {
        if (isQuizActive)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver();
            }
            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void nextstage()
    {
        SceneManager.LoadScene("Lab1");
    }

    public void GameOver()
    {
        isQuizActive = false;
        Quizpanel.SetActive(false);
        Resultpanel.SetActive(true);

        int finalScore = Mathf.RoundToInt(((float)score / totalQuestions) * 100);
        FinalScoreTxt.text = finalScore.ToString();

        ScoreTxt.text = score + "/" + totalQuestions;
        SeluruhSoalTxt.text = totalQuestions.ToString();
        JumlahBenarTxt.text = score.ToString();

        GameDataManager.Instance.SetFinalScore(score, answerLog);
    }

    public void correct()
    {
        score += 1;
        answerLog[currentQuestion] = 1;
        QnA.RemoveAt(currentQuestion);
        StartCoroutine(WaitForNext());
    }

    public void wrong()
    {
        answerLog[currentQuestion] = 0;
        QnA.RemoveAt(currentQuestion);
        timeRemaining -= 5f;
        if (timeRemaining < 0) timeRemaining = 0;
        StartCoroutine(WaitForNext());
    }

    IEnumerator WaitForNext()
    {
        yield return new WaitForSeconds(1);
        generateQuestion();
    }

    void generateQuestion()
    {
        if (QnA.Count > 0)
        {
            currentQuestion = UnityEngine.Random.Range(0, QnA.Count);
            QuestionImg.sprite = QnA[currentQuestion].Question;
            SetAnswers();
        }
        else
        {
            GameOver();
        }
    }

    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            var answerScript = options[i].GetComponent<AnswerScript>();

            options[i].GetComponent<Image>().color = answerScript.startColor;
            answerScript.isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Image>().sprite = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                answerScript.isCorrect = true;
            }
        }
    }
}
