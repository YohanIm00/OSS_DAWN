using UnityEngine;

public class DialogueCall : MonoBehaviour
{
    public DialogueController dialogController;
    public DialogueText dialog;
    private bool isTalkable = true;
    private bool isFirst = true;
    
    private void Update()
    {
        if(isTalkable) TutorialConversation();
        
    }
    public void Talk(DialogueText dialog)
    {
        dialogController.DisplayNextText(dialog);
    }

    public void TutorialConversation()
    {
            if (Input.GetKeyDown(KeyCode.Space) || isFirst)
            {
                Debug.Log("처음");
                isFirst = false;
                Talk(dialog);
            }
            if (!DialogueController.IsConversation)
            {
                Debug.Log("끝");
                isTalkable = true;
            }
    }
}
