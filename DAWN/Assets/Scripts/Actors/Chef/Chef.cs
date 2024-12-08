using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chef : NPC
{
    public Queue<MenuSO> orderQueue = new Queue<MenuSO>(10);
    public Queue<MenuSO> bakeQueue = new Queue<MenuSO>(4);
    public Queue<MenuSO> completeQueue = new Queue<MenuSO>(6);

    [SerializeField] private Slider oven1;
    [SerializeField] private Slider oven2;
    [SerializeField] private Slider oven3;
    [SerializeField] private Slider oven4;

    enum State { Cook, Break, Complete }

    public MenuSO menu;

    private State currentState = State.Break;

    int cookingCount = 0;

    private Coroutine baking1;
    private Coroutine baking2;
    private Coroutine baking3;
    private Coroutine baking4;

    public override void Interact()
    {
        GetOrder();
        if (currentState == State.Break)
            Break();
        else if (currentState == State.Complete)
            GiveMenu();
    }

    protected override void _Update()
    {
        if (orderQueue.Count > 0 && bakeQueue.Count < 4)
            bakeQueue.Enqueue(orderQueue.Dequeue()); // Start to Bake!
        if (bakeQueue.Count > 0)
        {
            if (baking1 == null)
                baking1 = StartCoroutine(Cooking(1));
            else if (baking2 == null)
                baking2 = StartCoroutine(Cooking(2));
            else if (baking3 == null)
                baking3 = StartCoroutine(Cooking(3));
            else if (baking4 == null)
                baking4 = StartCoroutine(Cooking(4));
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

    private void GiveMenu()
    {
        if (completeQueue.Count == 0)
            return;
        if (player.isFull)
            return;

        for (int i = 0; i < player.servingPaws.Count; ++i)
        {
            if (player.servingPaws[i] == null)
            {
                player.servingPaws[i] = completeQueue.Dequeue();
                Debug.Log("Let's serve " + player.servingPaws[i] + "!");
                player.DisplayServedFood(player.servingPaws[i], i, true);
                    break;
            }
        }
        
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
        if (cookingCount >= 4)
            yield break;
        
        Slider slider = null;

        // Settings for slider
        switch(slot)
        {
            case 1:
                slider = oven1.GetComponent<Slider>();
                oven1.gameObject.SetActive(true);
                break;
            case 2:
                slider = oven2.GetComponent<Slider>();
                oven2.gameObject.SetActive(true);
                break;
            case 3:
                slider = oven3.GetComponent<Slider>();
                oven3.gameObject.SetActive(true);
                break;
            case 4:
                slider = oven4.GetComponent<Slider>();
                oven4.gameObject.SetActive(true);
                break;
        }

        if (slider == null)
        {
            Debug.LogError("Slider is NULL. slot number: " + slot);
            yield break;
        }

        cookingCount++;
        MenuSO currentMenu = bakeQueue.Dequeue();
        StartCook(currentMenu);
        if (currentMenu.GetCookingTime() == 3)
            AudioManager.instance.PlaySfx(AudioManager.SFX.Baking3s);
        else
            AudioManager.instance.PlaySfx(AudioManager.SFX.Baking5s);

        Debug.Log("Start to bake " + currentMenu.name + "!");

        float duration = currentMenu.GetCookingTime();
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
        Debug.Log("It is done baking " + currentMenu.name + "!");
        completeQueue.Enqueue(currentMenu);
        Debug.Log($"{currentMenu.name} is put on completeQueue.");
        Debug.Log($"1st menu in completeQueue is {completeQueue.Peek()}");

        // Resetting the slider
        slider.value = 0;
        slider.gameObject.SetActive(false);

        switch(slot)
        {
            case 1:
                baking1 = null;
                player.cookingFood[0].SetActive(false);
                break;
            case 2:
                baking2 = null;
                player.cookingFood[1].SetActive(false);
                break;
            case 3:
                baking3 = null;
                player.cookingFood[2].SetActive(false);
                break;
            case 4:
                baking4 = null;
                player.cookingFood[3].SetActive(false);
                break;
        }


        FinishCook(currentMenu);
        cookingCount--;
    }
}
