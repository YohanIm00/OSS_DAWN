using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class AbstractFade : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected Tween fadeTween;

    void OnEnable()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Color temp = spriteRenderer.color;
        temp.a = 0f;
        spriteRenderer.color = temp;
        StartCoroutine(Fade());
    }
    protected abstract IEnumerator Fade();
}
