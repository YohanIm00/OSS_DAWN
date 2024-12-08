using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LightTransform : AbstractParts
{
    protected override IEnumerator Alter()
    {
        Color temp = image.color;
        temp.a = 0f;
        image.color = temp;

        yield return image.DOFade(0.6f, 0f);
        yield return new WaitForSeconds(0.03f);
        yield return image.DOFade(0f, 0f);
        AudioManager.instance.PlaySfx(AudioManager.SFX.Spark);
        yield return new WaitForSeconds(0.05f);
        yield return image.DOFade(0.8f, 0f);
        yield return new WaitForSeconds(0.07f);
        yield return image.DOFade(0f, 0f);

        yield return new WaitForSeconds(1.35f);
        AudioManager.instance.PlaySfx(AudioManager.SFX.Spotlight);
        yield return image.DOFade(1f, 0f);
    }
}
