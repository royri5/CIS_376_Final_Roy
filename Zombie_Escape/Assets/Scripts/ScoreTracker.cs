using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    //public Slider healthBar;
    public Text scoreText;
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        //healthBar.value = player.GetComponent<PlayerController>().health;
        scoreText.text = "Score: " + player.GetComponent<PlayerController>().score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //healthBar.value = player.GetComponent<PlayerController>().health;
        scoreText.text = "Score: " + player.GetComponent<PlayerController>().score.ToString();
    }
}
