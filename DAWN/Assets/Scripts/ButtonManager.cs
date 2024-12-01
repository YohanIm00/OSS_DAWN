using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void btn_Play() {
        // SceneManager.LoadScene("Cutscene0");
        print("Game Start");  // Validating whether each buttons are working well or not by printing each sentence on the terminal
    }

    public void btn_Config() {
        // SceneManager.LoadScene("Setting");
        print("Configuration");
    }

    public void btn_Quit() {
        Application.Quit();
    }

    public void btn_MainScene() {
        // SceneManager.LoadScene("MainMenu");
        print("Return to MainMenu");
    }
}