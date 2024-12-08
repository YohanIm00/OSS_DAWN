using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // GameDataSO gameDataSO;

    public void btn_Play() 
    {
        SceneManager.LoadScene("Prologue");
        print("Game Start");  // Validating whether each buttons are working well or not by printing each sentence on the terminal
    }

    public void btn_Config() { }

    public void btn_Quit() 
    {
        Application.Quit();
    }

    public void btn_MainScene() 
    {
        SceneManager.LoadScene("MainMenu");
        print("Return to MainMenu");
    }
}