using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "playerServing")]
public class PlayerServingSO : PlayerStateSO
{   
    public override void Execute(PlayerController playerController, PlayerAction playerAction)
    {
        if (playerAction.hitCustomer.isOrdered)
        {
            if (playerAction.hitCustomer.menu == playerController.servingMenu)
            {
                Debug.Log("Thanks for serving " + playerController.servingMenu);
                playerAction.hitCustomer.isReceived = true;
                playerController.DisplayServedFood(playerAction.hitCustomer.menu, false);
            }
            else
                Debug.Log("I didn't order this menu: " + playerController.servingMenu);
        }
    }
    public override void Enter() {}
    public override void Exit() {}
}
