using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiSO : ScriptableObject
{
    [SerializeField] private Sprite emoji;
    public virtual Sprite GetSprite() { return emoji; }
}
