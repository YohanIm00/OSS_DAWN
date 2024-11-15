using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeFunc : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Tween fadeTween;

    void OnEnable()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Color temp = spriteRenderer.color;
        temp.a = 0f;
        spriteRenderer.color = temp;
        StartCoroutine(FadeIn());
    }
    private IEnumerator FadeIn()
    {
        yield return spriteRenderer.DOFade(1,1f).WaitForCompletion();
    }

    // private IEnumerator FadeIn() // Activate later when it would be used.
    // {
    //     yield return spriteRenderer.DOFade(0,1f).WaitForCompletion();
    // }
}
