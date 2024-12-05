using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameDataSO gameDataSO;

    [Header("Data")]
    public bool isGame = false;
    public float totalGameTime = 75;
    public int totalBalloon = 1000;
    public int currentBalloon = 0;
    // Audio would be managed by AudioManager later

    [Header("UI")]  // It might be better to create UIManager later
    public TMP_Text txt_timer;
    public TMP_Text txt_balloon;
    public Slider sld_balloon;
    public GameObject finish;

    public List<GameObject> customers = new List<GameObject>();

    private void Awake() 
    { 
            instance = this;
            this.currentBalloon = gameDataSO.currentBalloon;
    }

    private void Start() { sld_balloon.maxValue = totalBalloon; }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isGame)
        {
            UpdateTimer();
            UpdateBalloon();
        }
    }

    public void GainBalloon(bool plus)
    {
        if (plus)
            currentBalloon += Random.Range(3, 8);
        else
        {
            currentBalloon -= Random.Range(1, 4);
            if (currentBalloon < 0)
                currentBalloon = 0;
        }
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
        totalGameTime -= Time.deltaTime;
        if (totalGameTime < 0 && instance.customers.Count <= 0)
        {
            isGame = false;
            if (totalBalloon > currentBalloon)
                StartCoroutine(ToResultFail());
            else
                StartCoroutine(ToResultClear());
        }
        else
            txt_timer.text = Mathf.Max(totalGameTime, 0).ToString("N0");
    }

    void UpdateBalloon()
    {
        sld_balloon.value = currentBalloon;
        txt_balloon.text = $"{currentBalloon} / {totalBalloon}";
    }
}
