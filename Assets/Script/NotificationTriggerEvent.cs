using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationTriggerEvent : MonoBehaviour
{
   [Header("UI Content")]
   [SerializeField] private TextMeshProUGUI NotificationTextUI;

   [Header("Message Customisation")]
   [SerializeField] [TextArea] private string NotificationMessage;

   [Header("Notification Removal")]
   [SerializeField] private bool removeAfterExit = false;
   [SerializeField] private bool disableAfterTimer = false;
   [SerializeField] float disableTimer = 1.0f;

   [Header("Notification Animation")]
   [SerializeField] private Animator notificationAnim;
   private BoxCollider objectCollider;

   private void Awake()
   {
    objectCollider = gameObject.GetComponent<BoxCollider>();
   }

   private void OnTriggerEnter(Collider other)
   {
    if (other.CompareTag("Player"))
    {
        StartCoroutine(EnableNotification());
    }
   }

   private void OnTriggerExit(Collider other)
   {
    if (other.CompareTag("Player") && removeAfterExit)
    {
        RemoveNotification();
    }
   }

   IEnumerator EnableNotification()
   {
    objectCollider.enabled = false;
    notificationAnim.Play("NotificationFadeIn");
    NotificationTextUI.text = NotificationMessage;
    
    if (disableAfterTimer)
    {
        yield return new WaitForSeconds(disableTimer);
        RemoveNotification();
    }
   }

   void RemoveNotification()
   {
    notificationAnim.Play("NotificationFadeOut");
    gameObject.SetActive(false);
   }

}