// Author: Richard Roy
// Date: April 21, 2025
// Description: Handles the end screen UI and functionality,
//              including replaying the game, returning to the main menu,
//              and quitting the game, as well as displaying
//              the win or lose message.
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    // Reload the game scene on button press
    public void Replay()
    {
        // Reset the time scale to normal
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(1);
    }
    // Load the main menu scene on button press
    public void MainMenu()
    {
        // Reset the time scale to normal
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(0);
    }
    // Quit the game on button press
    public void QuitGame()
    {
        Application.Quit();
    }
}