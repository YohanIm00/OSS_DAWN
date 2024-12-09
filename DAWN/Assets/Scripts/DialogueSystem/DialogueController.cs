using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
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

    private Queue<DialogueText.SpeakerData> dialogueQueue = new Queue<DialogueText.SpeakerData>();

    private bool isAllSaid = false;

    private static bool isConversation = false;
    public static bool IsConversation
    {
        get
        { 
            return isConversation;
        }
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
        // gameObject.SetActive(true);
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
            if(dialogueQueue.Peek().speakerName == "투명")
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);

            if(dialogueQueue.Peek().isIllust)
            {
                illustration.SetActive(true);
                wand.SetActive(false);
                pu.SetActive(false);
                if (dialogueQueue.Peek().illustSprite != null)
                    illustration.GetComponent<Image>().sprite = dialogueQueue.Peek().illustSprite;

                // if (dialogueQueue.Peek().illustSprite.name == "3")
                // {
                //     tiramisu.GetComponent<Image>().sprite = dialogueQueue.Peek().illustSprite;
                //     tiramisu.SetActive(true);
                // }
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
                illustration.SetActive(false);
                wand.SetActive(false);
                pu.SetActive(false);
            }
            
            temp = dialogueQueue.Dequeue();
            nameText.text = temp.speakerName;
            conversationText.text = temp.dialogueText;
            if (gameObject.activeSelf)
                typingRoutine = StartCoroutine(TypingRoutine());
        }

        if (dialogueQueue.Count == 0)
        {
            isAllSaid = true;
            // StartCoroutine(SceneChange());
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

    public int GetDialogueQSize()
    {
        return dialogueQueue.Count;
    }

    // IEnumerator SceneChange()
    // {
    //     yield return new WaitForSeconds(2f);
        
    //     SceneManager.LoadScene("MainGame");
    // }
}
