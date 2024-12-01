using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class AbstractParts : MonoBehaviour
{
    [SerializeField] protected Image image;
    protected Tween tween;

    void OnEnable()
    {
        image = gameObject.GetComponent<Image>();
        // Color temp = image.color;
        // temp.a = 0f;
        // image.color = temp;
        StartCoroutine(Alter());
    }
    protected abstract IEnumerator Alter();
}
