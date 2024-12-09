using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        
        currentBalloon = gameDataSO.currentBalloon;
        int goal = TOTAL_BALLOON - currentBalloon;
        int bias = Mathf.RoundToInt(goal * 0.01f) + Mathf.RoundToInt(currentBalloon * 0.01f);
        maxBalloon = goal / EXPECTED_NUM_OF_CUSTOMER + bias;
    }

    private void Start() 
    { 
        timerSlider.maxValue = TOTAL_GAME_TIME;
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

    public void GainBalloon(bool plus, float cookingDuration)
    {
        if (plus)
        {
            if (cookingDuration > 4)
                currentBalloon += Random.Range((int)(maxBalloon * 0.4f), (int)(maxBalloon * 1.2f));
            else
                currentBalloon += Random.Range((int)(maxBalloon * 0.6f), (int)(maxBalloon * 0.8f));
        }
        else
        {
            if (cookingDuration > 4)
                currentBalloon -= Random.Range((int)(maxBalloon * 0.2f), (int)(maxBalloon * 0.6f));
            else
                currentBalloon -= Random.Range((int)(maxBalloon * 0.3f), (int)(maxBalloon * 0.5f));
            
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
