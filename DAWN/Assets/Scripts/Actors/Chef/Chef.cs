using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chef : NPC
{
    public Queue<MenuSO> orderQueue = new Queue<MenuSO>(8);
    public Queue<MenuSO> bakeQueue = new Queue<MenuSO>(2);
    public Queue<MenuSO> completeQueue = new Queue<MenuSO>(5);

    [SerializeField] private Slider slider1;
    [SerializeField] private Slider slider2;

    enum State { Cook, Break, Complete }

    public MenuSO menu;

    private State currentState = State.Break;

    int cookingCount = 0;

    private Coroutine cooking1;
    private Coroutine cooking2;

    public override void Interact()
    {
        GetOrder();
        if (currentState == State.Break)
            Break();
        else if (currentState == State.Complete)
            GiveCook();
    }

    protected override void _Update()
    {
        if (orderQueue.Count > 0 && bakeQueue.Count < 2)
            bakeQueue.Enqueue(orderQueue.Dequeue()); // Start to Bake!
        if (bakeQueue.Count > 0)
        {
            if (cooking1 == null)
                cooking1 = StartCoroutine(Cooking(1));
            else if (cooking2 == null)
                cooking2 = StartCoroutine(Cooking(2));
        }
    }

    private void GetOrder()
    {
        while (player.receiptQueue.Count > 0)
        {
                orderQueue.Enqueue(player.receiptQueue.Dequeue());
                Debug.Log($"{orderQueue.Peek()} is added to orderQueue");
        }
        
    }

    private void GiveCook()
    {
        if (completeQueue.Count == 0)
            return;
        if (player.isServing)
            return;

        //todo) Change this part after implementing serving List<menuSO> in playerController
        player.servingMenu = completeQueue.Dequeue();
        Debug.Log("Let's serve " + player.servingMenu + "!");
        player.DisplayServedFood(player.servingMenu, true);
        PickComplete();
    }

    private void Break() 
    { 
        currentState = State.Cook;
    }
    
    private void FinishCook(MenuSO currentMenu)
    {
        foreach (var item in player.completeFood)
        {
            if (item.activeSelf == false)
            {
                player.servingQueue.Enqueue(item);
                item.SetActive(true);
                SpriteRenderer spriteRenderer =item.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                    Sprite chosenSprite = CurrentSpriteChoicer(currentMenu);
                    if (chosenSprite != null)
                    {
                        spriteRenderer.sprite = chosenSprite;
                        Debug.Log("Sprite is successfully changed.");
                    }
                    else
                        Debug.LogWarning("Sprite is NULL.");
                }
                else
                    Debug.LogError("SpriteRenderer Component isn't available.");
                break;
            }
        }
    }

    private Sprite CurrentSpriteChoicer(MenuSO currentMenu)
    { 
        return currentMenu.GetSprite();
    }

    private void PickComplete() 
    { 
        player.servingQueue.Dequeue().SetActive(false); 
    }

    private void StartCook(MenuSO currentMenu)
    {
        foreach (var item in player.cookingFood)
        {
            if (item.activeSelf == false)
            {
                item.SetActive(true);
                SpriteRenderer spriteRenderer =item.GetComponent<SpriteRenderer>();
                if(spriteRenderer != null)
                {
                    Sprite chosenSprite = CurrentSpriteChoicer(currentMenu);
                    if (chosenSprite != null)
                    {
                        spriteRenderer.sprite = chosenSprite;
                        Debug.Log("Sprite is successfully changed.");
                    }
                    else
                        Debug.LogWarning("Sprite is NULL.");
                }
                else
                    Debug.LogError("SpriteRenderer Component isn't available.");
                break;
            }
        }
    }

    IEnumerator Cooking(int slot)
    {
        if (cookingCount >= 2)
            yield break;
        
        Slider slider = null;

        // Settings for slider
        if (slot == 1)
        {
            slider = slider1.GetComponent<Slider>();
            slider1.gameObject.SetActive(true);
        }
        else if (slot == 2)
        {
            slider = slider2.GetComponent<Slider>();
            slider2.gameObject.SetActive(true);
        }

        if (slider == null)
        {
            Debug.LogError("Slider is NULL. slot number: " + slot);
            yield break;
        }

        cookingCount++;
        MenuSO currentCook = bakeQueue.Dequeue();
        StartCook(currentCook);

        Debug.Log("Start to bake " + currentCook.name + "!");

        float duration = currentCook.GetCookingTime();
        slider.maxValue = duration; // Setting the maximum value of slider

        float elapsedTime = 0f;
        slider.maxValue = duration;  // Why is this assignment happening twice?

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            slider.value = elapsedTime; // Setting the current value of slider as the value of elapsedTime
            yield return null;
        }

        currentState = State.Complete;
        Debug.Log("It is done baking " + currentCook.name + "!");
        completeQueue.Enqueue(currentCook);

        // Resetting the slider
        slider.value = 0;
        slider.gameObject.SetActive(false);

        if (slot == 1)
        {
            cooking1 = null;
            player.cookingFood[0].SetActive(false);
        }
        else if (slot == 2)
        {
            cooking2 = null;
            player.cookingFood[1].SetActive(false);
        }
        FinishCook(currentCook);
        cookingCount--;
    }
}
