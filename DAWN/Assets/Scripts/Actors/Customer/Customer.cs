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
    public Animator animator;

    private void Awake()
    {
        stateMachine = gameObject.AddComponent<CustomerStateMachine>();
        animator = GetComponent<Animator>();
        canvas.gameObject.SetActive(false);
    }

    public void init(Sit dst, bool reverse = false)
    {
        menu = DataManager.instance.GetRandomMenu();

        if (!reverse)
        {
            for (int i = 0; i < dst.wayPoints.Count; ++i)
                wayPoints.Add(dst.wayPoints[i]);
            wayPoints.Add(dst.transform);
            sit = dst;
            orderWaitingTime = Random.Range(10, 16);
            menuWaitingTime = menu.GetCookingTime() + Random.Range(12, 18);
        }
        else
        {
            for (int i = dst.wayPoints.Count - 1; i >= 0; --i)
                wayPoints.Add(dst.wayPoints[i]);
            wayPoints.Add(CustomerSpawner.instance.entrance);
        }
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
        GameManager.instance.customers.Remove(gameObject);
    }
}
