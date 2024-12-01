using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SortableObject : ScriptableObject
{
    public Sprite sprite;
    public string correctKey; // "Left" or "Right"
}

public class SortingGame_Tentative : MonoBehaviour
{
    [Header("UI Elements")]
    public Text scoreText;
    public Slider timerSlider;
    public Text comboText;
    public Text feverText;
    public GameObject feverEffect;

    [Header("Game Settings")]
    public float gameDuration = 60f;
    public int comboThreshold = 5;
    public float feverDuration = 5f;
    public int baseScore = 10;

    [Header("Conveyor Settings")]
    public Transform[] conveyorPoints; // Positions for objects on conveyor
    public SortableObject tiramisu;
    public SortableObject milk;
    public SortableObject tiramisuSet;

    private int score = 0;
    private float timer;
    private int comboCount = 0;
    private bool isFeverActive = false;
    private Queue<GameObject> conveyorQueue = new Queue<GameObject>();

    void Start()
    {
        timer = gameDuration;
        UpdateUI();
        feverEffect.SetActive(false);

        InitializeConveyor();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateUI();

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SortObject("Left");
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SortObject("Right");
            }

            if (timer <= 0)
            {
                EndGame();
            }
        }
    }

    public void SortObject(string key)
    {
        if (conveyorQueue.Count > 0)
        {
            GameObject frontObject = conveyorQueue.Dequeue();
            SortableObject sortable = frontObject.GetComponent<SortableData>().data;

            if (isFeverActive && sortable == tiramisuSet)
            {
                score += baseScore * 2; // Double score during Fever mode
            }
            else if (sortable.correctKey == key)
            {
                score += isFeverActive ? baseScore * 2 : baseScore;
                comboCount++;

                if (comboCount >= comboThreshold && !isFeverActive)
                {
                    StartCoroutine(ActivateFeverMode());
                }
            }
            else
            {
                comboCount = 0; // Reset combo on incorrect sort
            }

            Destroy(frontObject);
            ReplaceConveyorObjectImmediately();
            UpdateUI();
        }
    }

    private IEnumerator ActivateFeverMode()
    {
        isFeverActive = true;
        feverText.text = "Fever!";
        feverEffect.SetActive(true);

        // Replace all objects in queue with Tiramisu Set
        foreach (GameObject obj in conveyorQueue)
        {
            obj.GetComponent<SpriteRenderer>().sprite = tiramisuSet.sprite;
            obj.GetComponent<SortableData>().data = tiramisuSet;
        }

        yield return new WaitForSeconds(feverDuration);

        // Reset all objects to normal
        foreach (GameObject obj in conveyorQueue)
        {
            SortableObject randomObject = Random.Range(0, 2) == 0 ? tiramisu : milk;
            obj.GetComponent<SpriteRenderer>().sprite = randomObject.sprite;
            obj.GetComponent<SortableData>().data = randomObject;
        }

        feverText.text = "";
        feverEffect.SetActive(false);
        isFeverActive = false;
        comboCount = 0; // Reset combo after Fever ends
    }

    private void InitializeConveyor()
    {
        for (int i = 0; i < conveyorPoints.Length; i++)
        {
            SpawnConveyorObject(i);
        }
    }

    private void SpawnConveyorObject(int positionIndex)
    {
        SortableObject randomObject = Random.Range(0, 2) == 0 ? tiramisu : milk;
        GameObject newObject = new GameObject("ConveyorObject");
        SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();
        renderer.sprite = randomObject.sprite;

        SortableData data = newObject.AddComponent<SortableData>();
        data.data = randomObject;

        newObject.transform.position = conveyorPoints[positionIndex].position;
        float scaleFactor = 1f + (positionIndex * 0.1f); // Increase size for objects closer to user
        newObject.transform.localScale = Vector3.one * scaleFactor;
        conveyorQueue.Enqueue(newObject);
    }

    private void ReplaceConveyorObjectImmediately()
    {
        SpawnConveyorObject(conveyorPoints.Length - 1);

        // Reposition and resize remaining objects
        GameObject[] objects = conveyorQueue.ToArray();
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.position = conveyorPoints[i].position;
            float scaleFactor = 1f + (i * 0.1f);
            objects[i].transform.localScale = Vector3.one * scaleFactor;
        }
    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        timerSlider.value = timer / gameDuration;
        comboText.text = "Combo: " + comboCount;
    }

    private void EndGame()
    {
        timer = 0;
        Debug.Log("Game Over! Final Score: " + score);
        // Implement end game logic here, such as showing a result screen or restarting.
    }
}

public class SortableData : MonoBehaviour
{
    public SortableObject data;
}

