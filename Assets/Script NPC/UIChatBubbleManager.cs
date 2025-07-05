using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChatBubbleManager : MonoBehaviour
{
    public static UIChatBubbleManager Instance { get; private set; }

    [SerializeField] private Dialogue bubblePrefab;
    [SerializeField] private Transform canvasTransform;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowBubble(Transform target, string[] lines)
    {
        Dialogue bubble = Instantiate(bubblePrefab, canvasTransform);
        bubble.gameObject.SetActive(true); // pastikan aktif

        bubble.SetTarget(target); // arahkan bubble ke NPC
        bubble.StartDialogue(lines);
    }
}
