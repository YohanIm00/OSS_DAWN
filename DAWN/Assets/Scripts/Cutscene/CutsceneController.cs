using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    private bool isTimelineFinished = false;
    private bool isFirst = true;
    [SerializeField] Text text;

    void Start()
    {
        // Subscribing to an event that triggers when the timeline stops playing
        playableDirector.stopped += OnPlayableDirectorStopped;
    }

    void Update()
    {
        // Checks if the timeline has finished and it's the first time this condition is met
        if (isTimelineFinished && isFirst)
        {
            isFirst = false;
            text.gameObject.SetActive(true); // Displays text once the cutscene ends
        }

        // Waits for player input (pressing Space) to load the next scene
        if (isTimelineFinished && Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextScene();
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        // Sets the flag to true when the cutscene is finished
        isTimelineFinished = true;
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("SortingGame");
    }
}
