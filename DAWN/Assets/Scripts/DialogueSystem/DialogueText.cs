using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DialogueSO")]
public class DialogueText : ScriptableObject
{
    [System.Serializable]
    public class SpeakerData
    {
        public string speakerName;

        [TextArea(5, 10)]
        public string dialogueText;
    }
    public SpeakerData[] speakerData;
}
