// Author: Richard Roy
// Date: April 21, 2025
// Description: Handles the random spawning of zombies
//              at the start of the game.
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombie;
    public int numberOfZombies = 30;
    public int zombiesLeft = 30;
    // Spawn all zombies randomly once at the start of the game
    void Update()
    {
        if (zombiesLeft > 0)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
            Instantiate(zombie, randomPosition, Quaternion.identity);
            zombiesLeft--;
        }
    }
}
