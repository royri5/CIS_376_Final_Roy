using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        healthBar.value = player.GetComponent<PlayerController>().health;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = player.GetComponent<PlayerController>().health;
    }
}
