using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ImageFadeIn : AbstractParts
{
    protected override IEnumerator Alter()
    {
        Color temp = image.color;
        temp.a = 0f;
        image.color = temp;

        yield return image.DOFade(1f, 3f);
    }
}
