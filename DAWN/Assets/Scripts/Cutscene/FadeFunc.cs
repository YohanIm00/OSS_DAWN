using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeFunc : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private Tween fadeTween;

    public void FadeInsert(float duration)
    {
        Fade(1f, duration, () => 
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }

    // public void FadeOut()    // Activate later when it would be used.
    // {
    //     Fade(1f, duration, () => 
    //     {
    //         canvasGroup.interactable = false;
    //         canvasGroup.blocksRaycasts = false;
    //     });
    // }

    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if (fadeTween != null)
            fadeTween.Kill(false);

            fadeTween = canvasGroup.DOFade(endValue, duration);
            fadeTween.onComplete += onEnd; 
    }
}
