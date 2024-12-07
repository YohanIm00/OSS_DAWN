using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Queue<MenuSO> receiptQueue = new Queue<MenuSO>(10);
    public Queue<GameObject> servingQueue = new Queue<GameObject>(10);
    public List<MenuSO> servingPaws = new List<MenuSO>(2);
    public GameObject[] completeFood = new GameObject[6];   // Points to display completed menus
    public GameObject[] cookingFood = new GameObject[4];    // Points to display currently cooking menus
    public GameObject[] servingFood = new GameObject[2];    // Points to display serving menus on Wand's paws
    private PlayerAction _playerAction;
    public PlayerStateMachine playerStateMachine;
    public bool isServing 
    {
        get 
        {
            return servingPaws[0] != null || servingPaws[1] != null;
        }

        private set {}
    }

    public bool isFull 
    { 
        get 
        {
            return servingPaws[0] != null && servingPaws[1] != null;
        } 

        private set {}
    }

    // public void StartServing(MenuSO servingMenu)
    // {
    //     this.servingMenu = servingMenu;
    //     DisplayServedFood(this.servingMenu, true);
    //     isServing = true;
    // }

    // public void EndServing()
    // {
    //     servingMenu = null;
    //     DisplayServedFood(servingMenu, true);
    //     isServing = false;
    // }

    public void DisplayServedFood(MenuSO servingMenu, int index, bool isDisplay)
    {
        if (isDisplay)
        {
            playerStateMachine.TransitionTo(playerStateMachine.servingState);
            servingFood[index].SetActive(true);
            servingFood[index].GetComponent<Image>().sprite = servingMenu.GetSprite();
        }
        else
        {
            servingFood[index].SetActive(false);
            servingFood[index].GetComponent<Image>().sprite = null;
            servingPaws[index] = null;
            if (!isServing)
                playerStateMachine.TransitionTo(playerStateMachine.waitingState);
        }
    }
    
    private void Start()
    {
        _playerAction = GetComponent<PlayerAction>();
        playerStateMachine.Initialize();

        for (int i = 0; i < 2; ++i)
            servingPaws.Add(null);
        
        isServing = false;
        isFull = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isServing)
            Munch(0);
        else if(Input.GetKeyDown(KeyCode.G) && isServing)
            Munch(1);

        if (Input.GetKeyDown(KeyCode.E) && _playerAction.hit.collider != null)
        {
            _playerAction.hitCustomer = _playerAction.hit.collider.GetComponent<Customer>();
            playerStateMachine.Update(this, _playerAction);
        }
    }

    private void Munch(int index)
    {
        DisplayServedFood(servingPaws[index], index, false);
        isFull = false;
    }
}
