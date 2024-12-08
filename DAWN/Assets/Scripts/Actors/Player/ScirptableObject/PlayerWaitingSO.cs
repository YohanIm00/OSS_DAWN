using UnityEngine;

[CreateAssetMenu(menuName = "playerWaiting")]
public class PlayerWaitingSO : PlayerStateSO
{   
    public override void Execute(PlayerController playerController, PlayerAction playerAction)
    {
        if (!playerAction.hitCustomer.isOrdered)
        {
            Debug.Log("Hit: " + playerAction.hit.collider.name);
            playerAction.hitCustomer.isOrdered = true;
            playerController.receiptQueue.Enqueue(playerAction.hitCustomer.menu);

            Debug.Log("Ordered Menu: " + playerController.receiptQueue.Peek());
        }
    }
    public override void Enter() {}
    public override void Exit() {}
}
