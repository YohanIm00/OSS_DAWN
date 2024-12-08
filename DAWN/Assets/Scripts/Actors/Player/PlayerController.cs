using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Queue<MenuSO> receiptQueue = new Queue<MenuSO>(10);
    public Queue<GameObject> servingQueue = new Queue<GameObject>(10);
    public List<MenuSO> servingPaws = new List<MenuSO>(2);
    public GameObject[] completeFood = new GameObject[6];   // Points to display completed menus
    public GameObject[] cookingFood = new GameObject[4];    // Points to display currently cooking menus
    public GameObject[] servingFood = new GameObject[2];    // Points to display serving menus on Wand's paws
    public PlayerAction playerAction;
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
    
    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        playerAction = GetComponent<PlayerAction>();
        playerStateMachine.Initialize();

        for (int i = 0; i < 2; ++i)
            servingPaws.Add(null);
        
        isServing = false;
        isFull = false;
    }

    private void Update()
    {
        if (isServing && GameManager.instance.currentSatiety < 100)
        {
            if (Input.GetKeyDown(KeyCode.F) && isServing)
                Munch(0);
            else if(Input.GetKeyDown(KeyCode.G) && isServing)
                Munch(1);
        }

        if (Input.GetKeyDown(KeyCode.E) && playerAction.hit.collider != null)
        {
            playerAction.hitCustomer = playerAction.hit.collider.GetComponent<Customer>();
            playerStateMachine.Update(this, playerAction);
        }
    }

    private void Munch(int index)
    {
        DisplayServedFood(servingPaws[index], index, false);
        GameManager.instance.GainSatiety();
        isFull = false;
    }
}
