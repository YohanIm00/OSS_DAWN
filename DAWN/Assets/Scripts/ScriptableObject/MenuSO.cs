using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuSO : ScriptableObject
{
    [SerializeField]
    protected float cookingDuration;
    [SerializeField]
    protected Sprite foodSprite;

    public virtual float GetCookingTime() { return cookingDuration; }

    public virtual Sprite GetSprite() { return foodSprite; }
}
