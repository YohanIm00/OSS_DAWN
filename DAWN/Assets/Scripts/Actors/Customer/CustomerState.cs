using UnityEngine;
using UnityEngine.UI;

public abstract class CustomerState : MonoBehaviour
{
    protected CustomerStateMachine stateMachine;
    protected Customer customer;
    protected Vector2 direction = Vector2.zero;

    public virtual void Enter(CustomerStateMachine stateMachine)
    {
        Debug.Log($"State : {this.GetType().Name}");
        this.stateMachine = stateMachine;
        customer = GetComponent<Customer>();
        customer.timer.fillAmount = 1;
    }

    public abstract void _Update();
    public abstract void Exit();

    public virtual void ShowEmoji(EmojiSO emoji)
    {
        customer.canvas.SetActive(true);
        customer.emoji.GetComponent<Image>().sprite = GetEmojiSprite(emoji);
        customer.timer.GetComponent<Image>().sprite = GetEmojiSprite(emoji);
    }

    Sprite GetEmojiSprite(EmojiSO emoji)
    {
        switch(emoji.name)
        {
            case "Order" :
                return DataManager.instance.emojis["Order"].GetSprite();
            case "Wait" :
                return customer.menu.GetSprite();
            case "Enjoy" :
                return DataManager.instance.emojis["Enjoy"].GetSprite();
            case "Anger" :
                return DataManager.instance.emojis["Anger"].GetSprite();
        }
        Debug.LogError("There is no corresponding sprite.");
        return null;
    }

    public virtual void HideEmoji() { customer.canvas.SetActive(false); }

    protected bool _horizontal;
    protected bool _vertical;

    protected void Animate()
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            _horizontal = true;
            _vertical = false;
            customer.animator.SetBool("isChanged", true);
        }
        if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            _horizontal = false;
            _vertical = true;
            customer.animator.SetBool("isChanged", true);
        }
        if (direction == Vector2.zero)
        {
            _horizontal = false;
            _vertical = false;
            customer.animator.SetBool("isChanged", true);
        }
        if (_horizontal != customer.animator.GetBool("isHori") || _vertical != customer.animator.GetBool("isVert"))
            customer.animator.SetBool("isChanged", false);
        
        customer.animator.SetBool("isHori", _horizontal);
        customer.animator.SetBool("isVert", _vertical);
        customer.animator.SetFloat("hori", direction.x);
        customer.animator.SetFloat("vert", direction.y);
    }
}
