using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public List<Transform> wayPoints = new List<Transform>();
    public CustomerStateMachine stateMachine;
    public Sit sit;
    public MenuSO menu;
    // Variables related to Ordering
    public float orderWaitingTime;
    public bool isOrdered = false;
    // Variables related to Serving
    public float menuWaitingTime;
    public bool isReceived = false;
    public float enjoyingTime = 2f;
    public readonly float moveSpeed = 3f;
    public GameObject canvas;
    public Image emoji;
    public Image timer;
    public Animator _anim;

    private void Awake()
    {
        stateMachine = gameObject.AddComponent<CustomerStateMachine>();
        _anim = GetComponent<Animator>();
        canvas.gameObject.SetActive(false);
    }

    public void init(Sit dst, bool reverse = false)
    {
        // implementing initializing each customer object
        // Create this part after implementing DataManager
    }

    public void Update()
    {
        stateMachine.currentState._Update();
    }

    public void ReceiveMenu(MenuSO menu)
    {
        if (this.menu.name != menu.name)
        {
            Debug.LogError("Menu name isn't same.");
            return;
        }
    }

    public void OnDestroy()
    {
        // Need to implement GameManager
    }
}
