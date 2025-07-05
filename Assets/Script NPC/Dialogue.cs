using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed = 0.05f;

    private string[] lines;
    private int index;

    private Transform target; // untuk mengikuti NPC

    void Update()
    {
        // Bubble mengikuti kepala NPC
        if (target != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 2f);
            transform.position = screenPos;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue(string[] newLines)
    {
        lines = newLines;
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

IEnumerator TypeLine()
{
    string[] words = lines[index].Split(' '); // Pisah berdasarkan spasi (kata)
    textComponent.text = "";

    foreach (string word in words)
    {
        textComponent.text += word + " ";
        yield return new WaitForSeconds(textSpeed);
    }
}

    void NextLine()
    {
        if (index < lines.Length-1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void LateUpdate()
{
    if (target != null)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 2f);
        transform.position = screenPos;
        Debug.Log("Bubble screen pos: " + screenPos);
    }
}

}
