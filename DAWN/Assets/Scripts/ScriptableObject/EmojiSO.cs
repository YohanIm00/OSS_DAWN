using UnityEngine;

[CreateAssetMenu(menuName = "emojiSO")]
public class EmojiSO : ScriptableObject
{
    [SerializeField] private Sprite emoji;
    public virtual Sprite GetSprite() { return emoji; }
}
