using System.Collections;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner instance;

    [Header("Positions")]
    public Sit[] sits;
    public Transform entrance;

    [Header("UIs")]
    public GameObject ready;
    public GameObject go;

    [Header("Prefabs")]
    public GameObject[] pre_customers;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        GameManager.instance.isGame = false;
        ready.SetActive(true);
        go.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        ready.SetActive(false);
        go.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        go.SetActive(false);
        GameManager.instance.isGame = true;
        while (GameManager.instance.isGame && GameManager.instance.totalGameTime > 0)
        {
            // SpawnCustomer();
            yield return new WaitForSeconds(Random.Range(0, 16));
        }
    }

    private void SpawnCustomer()
    {
        bool isFull = true;
        foreach (var s in sits)
            isFull &= s.isUsing;

        if (isFull)
        {
            print("There is no empty seat.");
            return;
        }

        int idx = Random.Range(0, sits.Length);
        Sit selectedSit = sits[idx];
        while (selectedSit.isUsing)
        {
            idx = Random.Range(0, sits.Length);
            selectedSit = sits[idx];
        }
        GameObject customer = Instantiate(pre_customers[Random.Range(0, pre_customers.Length)], entrance.position, Quaternion.identity);
        customer.GetComponent<Customer>().init(selectedSit);
        GameManager.instance.customers.Add(customer);
    }
}
