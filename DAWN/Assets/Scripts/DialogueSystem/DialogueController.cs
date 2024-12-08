using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using KoreanTyper;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI conversationText;
    [SerializeField] private GameObject forwardMark;

    [Header("Dialogue Objects")]
    [SerializeField] private GameObject wand;
    [SerializeField] private GameObject pu;
    [SerializeField] private GameObject illustration;
    [SerializeField] private GameObject tiramisu;   //todo SetActive() related task: implement later

    private Queue<DialogueText.SpeakerData> dialogueQueue = new Queue<DialogueText.SpeakerData>();

    private bool isAllSaid = false;

    private static bool isConversation = false;
    public static bool IsConversation
    {
        get { return isConversation; }
    }

    private DialogueText.SpeakerData temp;
    private Coroutine typingRoutine = null;

    void Awake()
    {
        wand.SetActive(false);
        pu.SetActive(false);
        illustration.SetActive(false);
    }
    public void DisplayNextText(DialogueText dialogueText)
    {
        forwardMark.SetActive(false);

        if (dialogueQueue.Count == 0 && typingRoutine == null)
        {
            if(!isAllSaid)
            {
                StartConversation(dialogueText);
            }
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
            if(dialogueQueue.Peek().isIllust)
            {
                illustration.SetActive(true);
                wand.SetActive(false);
                pu.SetActive(false);
                illustration.GetComponent<Image>().sprite = dialogueQueue.Peek().illustSprite;
            }
            else if (dialogueQueue.Peek().characterSprite != null)
            {
                illustration.SetActive(false);
                wand.SetActive(true);
                pu.SetActive(true);
                
                if(dialogueQueue.Peek().isWand)
                {
                    wand.GetComponent<Image>().sprite = dialogueQueue.Peek().characterSprite;
                }
                else
                {
                    pu.GetComponent<Image>().sprite = dialogueQueue.Peek().characterSprite;
                }
            }
            else
            {
                illustration.SetActive(true);
                wand.SetActive(false);
                pu.SetActive(false);
            }
            
            temp = dialogueQueue.Dequeue();
            nameText.text = temp.speakerName;
            conversationText.text = temp.dialogueText;
            typingRoutine = StartCoroutine(TypingRoutine());
        }

        if (dialogueQueue.Count == 0)
        {
            isAllSaid = true;
            SceneManager.LoadScene("MainGame");
        }
    }

    private void StartConversation(DialogueText dialogueText)
    {
        if (!gameObject.activeSelf)
        {
            
            isConversation = true;
            gameObject.SetActive(true);
        }
        Debug.Log("대화 시작");
        for (int i = 0; i < dialogueText.speakerData.Length; ++i)
            dialogueQueue.Enqueue(dialogueText.speakerData[i]);
    }

    private void EndConversation()
    {
        isAllSaid = false;
        Debug.Log("대화 끝");
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            isConversation = false;
        }
    }

    IEnumerator TypingRoutine()
    {
        temp.dialogueText = conversationText.text;

        int typingLength = temp.dialogueText.GetTypingLength();
        for (int index = 0; index <= typingLength; ++index)
        {
            conversationText.text = temp.dialogueText.Typing(index);
            yield return new WaitForSeconds(0.05f);
        }
        forwardMark.SetActive(true);
        typingRoutine = null;
    }
}
