using UnityEngine;
using UnityEngine.Rendering;
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
            case "0/*name these later" :
                // Implement this after implementing Data Manager?
                break;
        }
        Debug.LogError("There is no corresponding sprite.");
        return null;
    }

    public virtual void HideEmoji() { customer.canvas.SetActive(false); }

    protected bool _horizontal;
    protected bool _vertical;

    protected void Animate()
    {
        // Setting animator based on each objects direction
        // Implement this after creating animator
    }
}
