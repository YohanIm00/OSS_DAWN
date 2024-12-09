using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    private bool isTimelineFinished = false;
    private bool isFirst = true;

    void Start()
    {
        // Subscribing to an event that triggers when the timeline stops playing
        playableDirector.stopped += OnPlayableDirectorStopped;
    }

    void Update()
    {
        // Checks if the timeline has finished and it's the first time this condition is met
        if (isTimelineFinished && isFirst)
            isFirst = false;

        // Waits for player input (pressing Space) to load the next scene
        if (isTimelineFinished)
            StartCoroutine(LoadNextScene());
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        // Sets the flag to true when the cutscene is finished
        isTimelineFinished = true;
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}
