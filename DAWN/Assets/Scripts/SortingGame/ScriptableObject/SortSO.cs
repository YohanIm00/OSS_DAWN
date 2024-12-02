using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SortSO : ScriptableObject
{
    [SerializeField]
    protected string correctKey;
    [SerializeField]
    protected Sprite objectSprite;

    public virtual string GetKeyValue() { return correctKey; }

    public virtual Sprite GetSprite() { return objectSprite; }
}