using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LampTransform : AbstractParts
{
    protected override IEnumerator Alter()
    {
        // yield return image.DOFade(1f, 0f);
        yield return image.transform.DOMoveY(972, 1.5f, true);
    }
}
