// Author: Richard Roy
// Date: April 21, 2025
// Description: Handles the player's score HUD via a text object.
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    public Text scoreText;
    GameObject player;
    // Locate player object and set score text to player score
    void Start()
    {
        player = GameObject.Find("Player");
        scoreText.text = "Score: " + player.GetComponent<PlayerController>().score.ToString();
    }
    // Keep score text updated to player score
    void Update()
    {
        scoreText.text = "Score: " + player.GetComponent<PlayerController>().score.ToString();
    }
}
