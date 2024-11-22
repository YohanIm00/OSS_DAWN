using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public List<Transform> wayPoints = new List<Transform>();
    // Need to implement FiniteStateMachine
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
        // StateMachine
        _anim = GetComponent<Animator>();
        canvas.gameObject.SetActive(false);
    }

    public void init(Sit dst, bool reverse = false)
    {
        // implementing initializing each customer object
    }

    public void Update()
    {
        // connecting this method with FSM
    }

    public void ReceiveMenu(MenuSO menu)
    {
        // a method that distinguish whether each customer receive what they ordered or not
    }

    public void OnDestroy()
    {
        // Need to implement GameManager
    }
}
