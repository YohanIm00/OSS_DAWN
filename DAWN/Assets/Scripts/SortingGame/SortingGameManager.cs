using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SortingGameManager : MonoBehaviour
{
    [Header("Game Data SO")]
    [SerializeField] private GameDataSO gameDataSO;

    [Header("Data")]
    // Values for maintaining inputs
    public bool isGame = false;
    public bool isInputActivated = false;
    // Values for timer
    private const float TOTAL_GAME_TIME = 60;
    [SerializeField] private float currentGameTime = TOTAL_GAME_TIME;
    // Values for balloon score
    private const int BASE_SCORE = 1;
    private int currentBalloon = 0;
    // Values for combo system
    private const int COMBO_THRESHOLD = 20;
    private int comboCount = 0;
    // Values for fever system
    private const float FEVER_DURATION = 3f;
    private bool isFever = false;
    
    [Header("UI Elements")]
    public Slider timerSlider;
    public TMP_Text timerText;
    public TMP_Text balloonScore;
    public TMP_Text comboText;
    public TMP_Text feverText;
    // public GameObject feverEffect;
    public GameObject comboObject;
    public GameObject balloonObject;

    [Header("EyeCatchers")]
    public GameObject ready;
    public GameObject start;
    public GameObject finish;

    [Header("Conveyor Settings")]
    public GameObject[] commonPoints; // Positions for objects on conveyor
    public GameObject[] feverPoints; // Positions for objects on conveyor
    public Dictionary<string, SortSO> snacks = new Dictionary<string, SortSO>();
    private List<SortSO> currentConveyor = new List<SortSO>(7);

    private void Awake()
    {
        gameDataSO.Init();
        timerSlider.maxValue = TOTAL_GAME_TIME;

        LoadSnacks();
        InitConveyor();
    }

    private void Start()
    {
        StartCoroutine(GameStart());
    }

    void Update()
    {
        if (isInputActivated)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    SortObject("Left");
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    SortObject("Right");
            }
    }

    private void FixedUpdate()
    {
        if (isGame)
        {
            UpdateTimer();
            UpdateUI();
        }
    }

    IEnumerator GameStart()
    {
        ready.SetActive(true);
        start.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        
        ready.SetActive(false);
        start.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.SFX.SortStart);
        yield return new WaitForSeconds(1.5f);
        start.SetActive(false);

        comboObject.SetActive(true);
        balloonObject.SetActive(true);
        isGame = true;
        isInputActivated = true;
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

            if (isFever)
            {
                currentBalloon += BASE_SCORE * 2; // Double score during Fever mode
                AudioManager.instance.PlaySfx(AudioManager.SFX.CorrectSort);
            }
            else if (firstSnack.GetKeyValue() == key)
            {
                currentBalloon += isFever ? BASE_SCORE * 2 : BASE_SCORE;
                currentConveyor.RemoveAt(0);
                RefillConveyor();
                AudioManager.instance.PlaySfx(AudioManager.SFX.CorrectSort);
                comboCount++;

                if (comboCount >= COMBO_THRESHOLD && !isFever)
                {
                    StartCoroutine(ActivateFeverMode());
                }
            }
            else
            {
                comboCount = 0; // Reset combo on incorrect sort
                currentBalloon -= Mathf.RoundToInt(currentBalloon * 0.1f);
                AudioManager.instance.PlaySfx(AudioManager.SFX.WrongSort);
            }
        }
        else
            Debug.LogWarning("currentQueue is empty, nothing to sort!");
    }


    private IEnumerator ActivateFeverMode()
    {
        isFever = true;
        feverText.text = "Fever!!";
        // feverEffect.SetActive(true);

        yield return new WaitForSeconds(FEVER_DURATION);

        feverText.text = "";
        // feverEffect.SetActive(false);
        isFever = false;
        comboCount = 0; // Reset combo after Fever ends
    }

    private void InitConveyor()
    {
        for (int i = 0; i < feverPoints.Length; i++)
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

    private void RefillConveyor()
    {
        SortSO randomSnack = Random.Range(0, 2) == 0 ? snacks["Tiram"] : snacks["Milk"];
        currentConveyor.Add(randomSnack);

        for (int i = 0; i < commonPoints.Length; ++i)
            commonPoints[i].GetComponent<Image>().sprite = currentConveyor[i].GetSprite();
    }

    private void UpdateUI()
    {
        balloonScore.text = $"{currentBalloon}";
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

    void UpdateTimer()
    {
        currentGameTime -= Time.deltaTime;
        timerSlider.value = currentGameTime;

        if (currentGameTime < 0)
        {
            gameDataSO.currentBalloon = currentBalloon;
            Debug.Log("Game Over! Final Score: " + currentBalloon);

            isGame = false;
            isInputActivated = false;
            comboObject.SetActive(false);
            balloonObject.SetActive(false);

            StartCoroutine(GameFinish());
        }
        else
            timerText.text = Mathf.Max(currentGameTime, 0).ToString("N0");
    }

    IEnumerator GameFinish()
    {
        finish.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.SFX.SortFinish);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Dialogue");
    }
}