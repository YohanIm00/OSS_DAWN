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

    
    private bool isTellAll;

    private static bool isConversation = false;

    public static bool IsConversation
    {
        get { return isConversation; }
    }

    private Coroutine typingRoutine = null;

    public void DisplayNextText()
    {
        forwardMark.SetActive(false);

        // Implement these part after creating DialogueText
    }

    private void StartConversation()
    {
        // Implement these part after creating DialogueText
    }

    private void EndConversation()
    {
        isTellAll = false;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            isConversation = false;
        }
    }

    // IEnumerator TypingRoutine()
    // {
    //     // Implement these part after creating DialogueText
    // }
}
