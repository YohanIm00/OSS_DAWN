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
    public PlayerAction playerAction;
    public GameDataSO gameDataSO;

    [Header("Data")]
    // Values for Character movement
    public bool isGame = false;
    public bool isInputActivated = true;
    // Values for timer
    private const float TOTAL_GAME_TIME = 90;
    public float currentGameTime = TOTAL_GAME_TIME;
    private float timerValue = 0;
    // Values for score
    private const int EXPECTED_NUM_OF_CUSTOMER = 25;
    private const int TOTAL_BALLOON = 1000;
    public int currentBalloon = 0;
    [SerializeField] private int maxBalloon = 0;
    [SerializeField] private int goal;
    [SerializeField] private int bias;
    // Values for satiety
    public const float STOMACH_CAPACITY = 100;
    private const float PIECE_OF_CAKE = 60;
    public float currentSatiety = 0;

    // Audio would be managed by AudioManager later

    [Header("UI")]  // It might be better to create UIManager later
    // UIs for Timer
    public Slider timerSlider;
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
        
        // currentBalloon = gameDataSO.currentBalloon;
        FixData();
    }

    private void Start() 
    { 
        timerSlider.maxValue = TOTAL_GAME_TIME;
        balloonSlider.maxValue = TOTAL_BALLOON;
        satietySlider.maxValue = STOMACH_CAPACITY;
    }

    // private void Update()
    // {
    //     FixData();
    // }

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

    private void FixData()
    {
        goal = TOTAL_BALLOON - currentBalloon;
        bias = Mathf.RoundToInt(goal * 0.02f);
        maxBalloon = goal / EXPECTED_NUM_OF_CUSTOMER + bias;
    }

    public void GainBalloon(bool plus, float cookingDuration)
    {
        if (plus)
        {
            if (cookingDuration > 4)
                currentBalloon += Random.Range(Mathf.Max(1, Mathf.RoundToInt(maxBalloon * 0.6f)), Mathf.Max(4, Mathf.RoundToInt(maxBalloon * 1.4f)));
            else
                currentBalloon += Random.Range(Mathf.Max(2, Mathf.RoundToInt(maxBalloon * 0.9f)), Mathf.Max(3, maxBalloon));
        }
        else
        {
            if (cookingDuration > 4)
                currentBalloon -= Random.Range(Mathf.Max(2, Mathf.RoundToInt(maxBalloon * 0.5f)), Mathf.Max(8, Mathf.RoundToInt(maxBalloon * 1.5f)));
            else
                currentBalloon -= Random.Range(Mathf.Max(4, Mathf.RoundToInt(maxBalloon * 0.6f)), Mathf.Max(6, Mathf.RoundToInt(maxBalloon * 0.8f)));
            
            AudioManager.instance.PlaySfx(AudioManager.SFX.BalloonPop);

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
        AudioManager.instance.PlaySfx(AudioManager.SFX.MainFinish);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("GameClear");
    }

    IEnumerator ToResultFail()
    {
        finish.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.SFX.MainFinish);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("GameOver");
    }

    void UpdateTimer()
    {
        timerValue += Time.deltaTime;
        currentGameTime -= Time.deltaTime;
        timerSlider.value = timerValue;

        if (currentGameTime < 0 && instance.customers.Count <= 0)
        {
            isGame = false;
            isInputActivated = false;
            
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
        playerAction.SetSpeed(2);
        yield return new WaitForSeconds(1.5f);
        playerAction.SetSpeed(5);
        currentSatiety = 0;
        satietySlider.value = Mathf.Lerp(satietySlider.value, currentSatiety, Time.deltaTime * 10);
    }
}
