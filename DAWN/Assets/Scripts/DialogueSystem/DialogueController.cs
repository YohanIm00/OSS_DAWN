using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using KoreanTyper;
using UnityEngine.UIElements;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI conversationText;
    [SerializeField] private GameObject forwardMark;

    private Queue<DialogueText.SpeakerData> dialogueQueue = new Queue<DialogueText.SpeakerData>();

    private bool isAllSaid;

    private static bool isConversation = false;
    public static bool IsConversation
    {
        get { return isConversation; }
    }

    private DialogueText.SpeakerData temp;
    private Coroutine typingRoutine = null;

    public void DisplayNextText(DialogueText dialogueText)
    {
        forwardMark.SetActive(false);

        if (dialogueQueue.Count == 0 && typingRoutine == null)
        {
            if(!isAllSaid)
                StartConversation();
            else
            {
                EndConversation();
                return;
            }
        }

        if (typingRoutine != null)
        {
            StopCoroutine(typingRoutine);
            typingRoutine = null;
            conversationText.text = temp.dialogueText;
            forwardMark.SetActive(true);
            return;
        }

        if (dialogueQueue.Count > 0 && typingRoutine == null)
        {
            temp = dialogueQueue.Dequeue();
            nameText.text = temp.speakerName;
            conversationText.text = temp.dialogueText;
            typingRoutine = StartCoroutine(TypingRoutine());
        }
    }

    private void StartConversation()
    {
        // Implement these part after creating DialogueText
    }

    private void EndConversation()
    {
        isAllSaid = false;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            isConversation = false;
        }
    }

    IEnumerator TypingRoutine()
    {
        yield return null;
    }
}
