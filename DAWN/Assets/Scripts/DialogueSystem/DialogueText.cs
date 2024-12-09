using UnityEngine;

[CreateAssetMenu(menuName = "DialogueSO")]
public class DialogueText : ScriptableObject
{
    [System.Serializable]
    public class SpeakerData
    {
        public bool isIllust = false;
        public bool isWand = false;
        public Sprite characterSprite;
        public Sprite illustSprite;
        public string speakerName;

        [TextArea(5, 10)]
        public string dialogueText;
    }
    public SpeakerData[] speakerData;
}
