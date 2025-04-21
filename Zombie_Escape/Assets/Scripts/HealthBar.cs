// Author: Richard Roy
// Date: April 21, 2025
// Description: Handles the player's health bar HUD via a slider.
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    GameObject player;
    // Locate player object and set slider value to player health
    void Start()
    {
        player = GameObject.Find("Player");
        healthBar.value = player.GetComponent<PlayerController>().health;
    }
    // Keep slider value updated to player health
    void Update()
    {
        healthBar.value = player.GetComponent<PlayerController>().health;
    }
}
