using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeBuilding : AbstractFade
{
    protected override IEnumerator Fade()
    {
        for (int i = 0; i < 10; ++i)
        {
            yield return spriteRenderer.DOFade(0.8f,0.3f).WaitForCompletion();
            yield return spriteRenderer.DOFade(0.2f,0.3f).WaitForCompletion();
        }
    }
}
