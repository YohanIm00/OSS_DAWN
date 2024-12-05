using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SortingGame : MonoBehaviour
{
    public static SortingGame instance;
    
    [Header("Game Data SO")]
    [SerializeField] private GameDataSO gameDataSO;
    [Header("UI Elements")]
    public TMP_Text scoreText;
    public Slider timerSlider;
    public TMP_Text comboText;
    public TMP_Text feverText;
    // public GameObject feverEffect;

    [Header("Game Settings")]
    public float gameDuration = 60f;
    public int comboThreshold = 20;
    public float feverDuration = 3f;
    public int baseScore = 2;

    [Header("Conveyor Settings")]
    public GameObject[] commonPoints; // Positions for objects on conveyor
    public GameObject[] feverPoints; // Positions for objects on conveyor
    public Dictionary<string, SortSO> snacks = new Dictionary<string, SortSO>();
    private List<SortSO> currentConveyor = new List<SortSO>(8);

    private int score = 0;
    private float timer;
    private int comboCount = 0;
    private bool isFever = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        LoadSnacks();
        if (snacks.Count == 0)
        {
            Debug.LogError("Snacks dictionary is empty!");
            return;
        }

        timer = gameDuration;

        InitConveyor();
        if (currentConveyor == null)
        {
            Debug.LogError("currentQueue is not initialized!");
        }

        UpdateUI();
        // feverEffect.SetActive(false);
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

    private void LoadSnacks()
    {
        SortSO[] loadData = Resources.LoadAll<SortSO>("SortingSnacks");

        if (loadData.Length == 0)
        {
            Debug.LogError("No snacks found in Resources/SortingSnacks!");
        }

        foreach (SortSO snack in loadData)
        {
            snacks.Add(snack.name, snack);
            Debug.Log($"Loaded snack: {snack.name}");
        }
    }

    public void SortObject(string key)
    {
        if (currentConveyor.Count > 0)
        {
            SortSO firstSnack = currentConveyor[0];

            if (firstSnack == null)
            {
                Debug.LogError("Dequeued a null object from the currentQueue!");
                return;
            }

            if (isFever)
            {
                score += baseScore * 2; // Double score during Fever mode
            }
            else if (firstSnack.GetKeyValue() == key)
            {
                score += isFever ? baseScore * 2 : baseScore;
                currentConveyor.RemoveAt(0);
                FixedUpdateConveyor();
                comboCount++;

                if (comboCount >= comboThreshold && !isFever)
                {
                    StartCoroutine(ActivateFeverMode());
                }
            }
            else
            {
                comboCount = 0; // Reset combo on incorrect sort
            }

            UpdateUI();
        }
        else
        {
            Debug.LogWarning("currentQueue is empty, nothing to sort!");
        }
    }


    private IEnumerator ActivateFeverMode()
    {
        isFever = true;
        feverText.text = "Fever!";
        // feverEffect.SetActive(true);

        yield return new WaitForSeconds(feverDuration);

        feverText.text = "";
        // feverEffect.SetActive(false);
        isFever = false;
        comboCount = 0; // Reset combo after Fever ends
    }

    private void InitConveyor()
    {
        for (int i = 0; i < commonPoints.Length; i++)
        {
            feverPoints[i].GetComponent<Image>().sprite = snacks["TiramSet"].GetSprite();
        }

        for (int i = 0; i < commonPoints.Length; i++)
        {
            SortSO randomSnack = Random.Range(0, 2) == 0 ? snacks["Tiram"] : snacks["Milk"];
            currentConveyor.Add(randomSnack);
            Debug.Log($"Enqueued: {randomSnack.name}");
            commonPoints[i].GetComponent<Image>().sprite = randomSnack.GetSprite();
        }
    }

    private void FixedUpdateConveyor()
    {
        SortSO randomSnack = Random.Range(0, 2) == 0 ? snacks["Tiram"] : snacks["Milk"];
        currentConveyor.Add(randomSnack);
        for (int i = 0; i < commonPoints.Length; ++i)
        {
            commonPoints[i].GetComponent<Image>().sprite = currentConveyor[i].GetSprite();
        }
    }

    private void UpdateUI()
    {
        scoreText.text = $"{score}";
        timerSlider.value = timer / gameDuration;
        comboText.text = "Combo: " + comboCount;

        if (isFever)
        {
            for (int i = 0; i < feverPoints.Length; ++i)
            {
                feverPoints[i].SetActive(true);
                commonPoints[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < feverPoints.Length; ++i)
            {
                feverPoints[i].SetActive(false);
                commonPoints[i].SetActive(true);
            }
        }
    }

    private void EndGame()
    {
        timer = 0;
        Debug.Log("Game Over! Final Score: " + score);
        gameDataSO.currentBalloon = score;
        // Implement end game logic here, such as showing a result screen or restarting.
    }
}