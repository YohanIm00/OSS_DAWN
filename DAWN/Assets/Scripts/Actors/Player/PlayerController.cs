using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Queue<MenuSO> menuQueue = new Queue<MenuSO>(8);
    public Queue<GameObject> readyQueue = new Queue<GameObject>(8);
    public GameObject[] completeFood = new GameObject[5];
    public GameObject[] cookingFood = new GameObject[2];
    public MenuSO servingMenu;
    private PlayerAction playerAction;
    public PlayerStateMachine playerStateMachine;
    public bool isServing { get; private set; }
    [SerializeField] private GameObject servingObject;

    public void StartServing(MenuSO servingMenu)
    {
        this.servingMenu = servingMenu;
        DisplayServedFood(this.servingMenu, true);
        isServing = true;
    }

    public void EndServing(MenuSO servingMenu)
    {
        this.servingMenu = null;
        DisplayServedFood(this.servingMenu, true);
        isServing = false;
    }

    public void DisplayServedFood(MenuSO servingMenu, bool isDisplay)
    {
        if (isDisplay)
        {
            playerStateMachine.TransitionTo(playerStateMachine.servingState);
            servingObject.GetComponent<SpriteRenderer>().sprite = servingMenu.GetSprite();
        }
        else
        {
            playerStateMachine.TransitionTo(playerStateMachine.waitingState);
            servingObject.GetComponent<SpriteRenderer>().sprite = null;
            servingMenu = null;
        }
    }
    
    private void Start()
    {
        playerAction = GetComponent<PlayerAction>();
        playerStateMachine.Initialize();
        isServing = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))    // Each KeyCode can be changed later
            Throwaway();

        if (Input.GetKeyDown(KeyCode.E) && playerAction.hit.collider != null)
        {
            // Former part will be implemented after the Customer object
            // playerStateMachine.Update(this, playerAction);
        }
    }

    private void Throwaway()    // This function will be changed as "Eat" later
    {
        DisplayServedFood(servingMenu, false);
        servingMenu = null;
        isServing = false;
    }
}
