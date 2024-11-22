using UnityEngine;
using UnityEngine.UI;

public abstract class CustomerState : MonoBehaviour
{
    // Need to implement StateMachine
    protected Customer customer;
    protected Vector2 direction = Vector2.zero;

    public virtual void Enter(CustomerStateMachine customerStateMachine)
    {
        // Make customer to enter the bakery
    }

    public abstract void _Update();
    public abstract void Exit();

    public virtual void ShowEmoji(/*EmojiSO emoji*/)
    {
        // Set speech bubble active and showing each customer's emoji
    }

    Sprite GetEmojiSprite(/*EmojiSO emoji*/)
    {
        // Calling appropriate Emoji based on the current state
        return null;
    }

    public virtual void HideEmoji()
    {
        // Deactivate speech bubble
    }

    protected bool _horizontal;
    protected bool _vertical;

    protected void Animate()
    {
        // Setting animator based on each objects direction
    }


}
