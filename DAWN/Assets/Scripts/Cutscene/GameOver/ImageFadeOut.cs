using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ImageFadeOut : AbstractParts
{
    protected override IEnumerator Alter()
    {
        yield return image.DOFade(0f, 3f);
    }
}
