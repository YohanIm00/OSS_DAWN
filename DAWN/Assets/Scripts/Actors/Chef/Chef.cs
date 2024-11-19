using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Chef : NPC
{
    public Queue<MenuSO> WaitingQueue = new Queue<MenuSO>(8);
    public Queue<MenuSO> CookingQueue = new Queue<MenuSO>(2);
    public Queue<MenuSO> CompleteQueue = new Queue<MenuSO>(5);

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
        if (WaitingQueue.Count > 0 && CookingQueue.Count < 2)
            CookingQueue.Enqueue(WaitingQueue.Dequeue()); // Start to Bake!
        if (CookingQueue.Count > 0)
        {
            if (cooking1 == null)
                cooking1 = StartCoroutine(Cooking(1));
            if (cooking2 == null)
                cooking2 = StartCoroutine(Cooking(2));
        }
    }

    private void GetOrder()
    {
        while (player.menuQueue.Count > 0)
            WaitingQueue.Enqueue(player.menuQueue.Dequeue());
    }

    private void GiveCook()
    {
        if (CompleteQueue.Count == 0)
            return;
        if (player.isServing)
            return;

        player.servingMenu = CompleteQueue.Dequeue();
        Debug.Log("Let's serve " + player.servingMenu + "!");
        player.DisplayServedFood(player.servingMenu, true);
        PickComplete();
    }

    private void Break() { currentState = State.Cook; }
    
    private void FinishCook(MenuSO currentMenu)
    {
        foreach (var item in player.completeFood)
        {
            if (item.activeSelf == false)
            {
                player.readyQueue.Enqueue(item);
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

    private Sprite CurrentSpriteChoicer(MenuSO currentMenu) { return currentMenu.GetSprite(); }

    private void PickComplete() { player.readyQueue.Dequeue().SetActive(false); }

    private void StartCook(MenuSO currentMenu)
    {
            foreach (var item in player.completeFood)
        {
            if (item.activeSelf == false)
            {
                player.readyQueue.Enqueue(item);
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
        MenuSO currentCook = CookingQueue.Dequeue();
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
        CompleteQueue.Enqueue(currentCook);

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
