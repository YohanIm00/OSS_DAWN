using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class DialogueCall : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public DialogueController dialogController;
    public DialogueText dialog;
    private bool isTalkable = true;
    private bool isFirst = true;

    private void Update()
    {
        if (isTalkable)
            Communicate();
        else
            SceneManager.LoadScene("MainGame");
        
    }

    public void Talk(DialogueText dialog)
    {
        dialogController.DisplayNextText(dialog);
    }

    public void Communicate()
    {
        if (Input.GetKeyDown(KeyCode.Space) || isFirst)
        {
            Debug.Log("처음");
            isFirst = false;
            Talk(dialog);
        }
        if (dialogController.GetDialogueQSize() == 0)
        {
            Debug.Log("끝");
            isTalkable = false;
        }
    }
}