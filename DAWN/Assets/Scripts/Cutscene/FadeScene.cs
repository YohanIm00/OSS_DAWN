using System.Collections;
using DG.Tweening;

public class FadeScene : AbstractFade
{
    protected override IEnumerator Fade()
    {
        yield return spriteRenderer.DOFade(1,1f).WaitForCompletion();
    }
}
