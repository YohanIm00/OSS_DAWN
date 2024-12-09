using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LetterTransform : AbstractParts
{
    protected override IEnumerator Alter()
    {
        Transform transform = image.transform;
        yield return transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0), 0.5f, 10, 1f);
    }
}
