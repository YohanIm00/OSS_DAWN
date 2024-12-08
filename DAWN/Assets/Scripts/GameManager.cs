using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameDataSO gameDataSO;

    [Header("Data")]
    public bool isGame = false;
    // Values for timer
    private const float TOTAL_GAME_TIME = 90;
    public float currentGameTime = TOTAL_GAME_TIME;
    private float timerValue = 0;
    // Values for score
    [SerializeField] private const int EXPECTED_NUM_OF_CUSTOMER = 20;
    private const int TOTAL_BALLOON = 1000;
    [SerializeField] private int currentBalloon = 0;
    [SerializeField] private int maxBalloon = 0;
    // Values for satiety
    private const float STOMACH_CAPACITY = 100;
    private const float PIECE_OF_CAKE = 60;
    public float currentSatiety = 0;

    // Audio would be managed by AudioManager later

    [Header("UI")]  // It might be better to create UIManager later
    // UIs for Timer
    public Slider TimerSlider;
    public TMP_Text timerText;
    // UIs for score
    public Slider balloonSlider;
    public TMP_Text balloonText;
    // UIs for satiety
    public Slider satietySlider;
    // public TMP_Text satietyText;
    public GameObject finish;

    public List<GameObject> customers = new List<GameObject>();

    private void Awake() 
    { 
        if (instance == null)
            instance = this;
        currentBalloon = gameDataSO.currentBalloon;
        int bias = Mathf.RoundToInt((TOTAL_BALLOON - currentBalloon) * 0.01f);
        maxBalloon = (TOTAL_BALLOON - currentBalloon) / EXPECTED_NUM_OF_CUSTOMER + bias;
    }

    private void Start() 
    { 
        TimerSlider.maxValue = TOTAL_GAME_TIME;
        balloonSlider.maxValue = TOTAL_BALLOON;
        satietySlider.maxValue = STOMACH_CAPACITY;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isGame)
        {
            UpdateTimer();
            UpdateBalloon();
            UpdateSatiety();
        }
    }

    public void GainBalloon(bool plus)
    {
        if (plus)
            currentBalloon += Random.Range((int)(maxBalloon * 0.4f), maxBalloon);
        else
        {
            currentBalloon -= Random.Range((int)(maxBalloon * 0.2f), (int)(maxBalloon * 0.5f));
            if (currentBalloon < 0)
                currentBalloon = 0;
        }
    }

    public void GainSatiety() 
    { 
        currentSatiety += PIECE_OF_CAKE;
    }

    IEnumerator ToResultClear()
    {
        finish.SetActive(true);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Clear");
    }

    IEnumerator ToResultFail()
    {
        finish.SetActive(true);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Fail");
    }

    void UpdateTimer()
    {
        timerValue += Time.deltaTime;
        currentGameTime -= Time.deltaTime;
        TimerSlider.value = timerValue;

        if (currentGameTime < 0 && instance.customers.Count <= 0)
        {
            isGame = false;
            if (TOTAL_BALLOON > currentBalloon)
                StartCoroutine(ToResultFail());
            else
                StartCoroutine(ToResultClear());
        }
        else
            timerText.text = Mathf.Max(currentGameTime, 0).ToString("N0");
    }

    void UpdateBalloon()
    {
        balloonSlider.value = Mathf.Lerp(balloonSlider.value, currentBalloon, Time.deltaTime * 10);
        balloonText.text = $"{currentBalloon} / {TOTAL_BALLOON}";
    }

    void UpdateSatiety()
    {
        if (currentSatiety > 0)
        {
            if (currentSatiety >= STOMACH_CAPACITY)
                StartCoroutine(SpeedDown());
            currentSatiety -= Time.deltaTime * 0.5f;
            satietySlider.value = Mathf.Lerp(satietySlider.value, currentSatiety, Time.deltaTime * 10);
        }
        else
            currentSatiety = 0;
    }

    IEnumerator SpeedDown()
    {
        if (PlayerController.instance == null)
        {
            Debug.LogError("PlayerController instance is null. Ensure it's initialized.");
            yield break;
        }

        if (PlayerController.instance.playerAction == null)
        {
            Debug.LogError("PlayerAction is null. Ensure PlayerAction is attached to the PlayerController GameObject.");
            yield break;
        }

        PlayerController.instance.playerAction.SetSpeed(1);
        yield return new WaitForSeconds(3);
        PlayerController.instance.playerAction.SetSpeed(5);
        currentSatiety = 0;
        satietySlider.value = Mathf.Lerp(satietySlider.value, currentSatiety, Time.deltaTime * 10);
    }
}
