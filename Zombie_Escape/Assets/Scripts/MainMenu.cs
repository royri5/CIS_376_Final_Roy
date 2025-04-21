// Author: Richard Roy
// Date: April 21, 2025
// Description: Handles the main menu UI and functionality,
//              including starting the game and quitting the application.
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // play button loads the game scene
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    // quit button quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}