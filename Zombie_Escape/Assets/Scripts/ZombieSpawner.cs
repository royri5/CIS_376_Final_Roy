using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombie;
    public int numberOfZombies = 10;
    public int zombiesLeft = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (zombiesLeft > 0)
        {
            //think about y position and procedural generation
            Vector3 randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(zombie, randomPosition, Quaternion.identity);
            zombiesLeft--;
        }
    }
}
