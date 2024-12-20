using UnityEngine;

[CreateAssetMenu(menuName = "playerServing")]
public class PlayerServingSO : PlayerStateSO
{   
    public override void Execute(PlayerController playerController, PlayerAction playerAction)
    {
        if (playerAction.hitCustomer.isOrdered)
        {
            for (int i = 0; i < playerController.servingPaws.Count; ++i)
            {
                if (playerAction.hitCustomer.menu == playerController.servingPaws[i])
                {
                    Debug.Log("Thanks for serving " + playerController.servingPaws[i]);
                    playerAction.hitCustomer.isReceived = true;
                    AudioManager.instance.PlaySfx(AudioManager.SFX.Serving);
                    playerController.DisplayServedFood(playerAction.hitCustomer.menu, i, false);
                    break;
                }
                else
                    Debug.Log("I didn't order this menu: " + playerController.servingPaws[i]);
            }
        }
    }
    public override void Enter() {}
    public override void Exit() {}
}
