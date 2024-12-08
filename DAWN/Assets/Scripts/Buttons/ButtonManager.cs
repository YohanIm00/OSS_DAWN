using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ButtonType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;

    public void Start()
    {
        defaultScale = buttonScale.localScale;
    }

    public void OnClick()
    {
        StartCoroutine(ButtonAction());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlaySfx(AudioManager.SFX.ButtonSelect);
        buttonScale.localScale = defaultScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }

    IEnumerator ButtonAction()
    {
        AudioManager.instance.PlaySfx(AudioManager.SFX.ButtonClick);
        
        yield return new WaitForSeconds(1f);

        switch (currentType)
        {
            case ButtonType.Play:
                SceneManager.LoadScene("Prologue");
                break;
            case ButtonType.Quit:
                Application.Quit();
                break;
            case ButtonType.Retry:
                SceneManager.LoadScene("MainGame");
                break;
            case ButtonType.Return:
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }
}