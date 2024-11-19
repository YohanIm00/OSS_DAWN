using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*public Queue<MenuSO> menuQueue = new Queue<MenuSO>(8);*/
    public Queue<GameObject> readyQueue = new Queue<GameObject>(8);
    public GameObject[] completeFood = new GameObject[5];
    public GameObject[] cookingFood = new GameObject[2];
    /*public MenuSO servingMenu;*/
    private PlayerAction playerAction;
    // public PlayerStateMachine playerStateMachine;
    public bool isServing { get; private set; }
    [SerializeField] private GameObject servingObject;

    public void StartServing(/*MenuSO servingMenu*/)
    {
        // This will be filled after MenuSO is implemented.
    }

    public void EndServing(/*MenuSO servingMenu*/)
    {
        // This will be filled after MenuSO is implemented.
    }

    public void DisplayServedFood(/*MenuSO servingMenu,*/ bool isDisplay)
    {
        // This will be filled after MenuSO is implemented.
    }
    
    private void Start()
    {
        playerAction = GetComponent<PlayerAction>();
        // PlayerStateMachine.Init()?;
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
        DisplayServedFood(/*servingMenu,*/ false);
        // servingMenu = null;
        isServing = false;
    }
}
