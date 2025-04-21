// Author: Richard Roy
// Date: April 21, 2025
// Reference: https://www.reddit.com/r/Unity3D/comments/8k7w7v/unity_simple_mouselook/
// Reference: https://discussions.unity.com/t/how-do-i-move-a-camera-with-mouse/194032/2
// Reference: https://www.youtube.com/watch?v=f473C43s8nE
// Description: Controller for most of the player's funcionality
//              Handles player movement, camera control, combat,
//              and health, as well as enabling end game menus
//              and score tracking.
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // UI elements
    public Text endText;
    public GameObject endMenu;
    public int health = 100;
    public int score = 0;
    public int maxScore = 10;

    // Player movement variables
    [Header("Player Movement")]
    public float speed = 5.0f;
    public float groundDrag = 5.0f;
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    // Camera and mouse look variables
    public float sensitivity;
    Vector2 rotation = Vector2.zero;
    public Camera cam;

    // Combat variables
    public GameObject bullet;
    private float rateOfFire = 0.5f;
    private float timeToFire = 0.5f;

    // Player variables
    Transform tr;
    public GameObject player;
    public Rigidbody rb;


    private bool running = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // handle cursor visibility in game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // get player object transform
        tr = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // game state checks
        if (!running) {
            return;
        }
        // check score to see if we win
        if (score >= maxScore) {
            // pause world
            Time.timeScale = 0.0f;
            // bring up end screen
            endMenu.SetActive(true);
            endText.text = "You win!";
            // unlock cursor for menu
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // game is over
            running = false;
        }
        // check hp to see if we lose
        if (health <= 0) {
            // pause world
            Time.timeScale = 0.0f;
            // bring up end screen
            endMenu.SetActive(true);
            endText.text = "You lose!";
            // unlock cursor for menu
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // game is over
            running = false;
        }

        // Movement and camera control
        // check if grounded for drag
        grounded = Physics.Raycast(tr.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (grounded) {
            // apply drag if grounded
            rb.linearDamping = groundDrag;
        }
        else {
            rb.linearDamping = 0;
        }
        // speed should not exceed cap
        Vector3 velocity = new Vector3(rb.linearVelocity.x, 0.0f, rb.linearVelocity.z);
        if (velocity.magnitude > speed) {
            Vector3 newVelocity = velocity.normalized * speed;
            rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);
        }
        // get user input for movement and camera
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity;
        // camera rotation
        rotation.y += mouseX;
        rotation.x -= mouseY;
        // clamp vertical rotation
        rotation.x = Mathf.Clamp(rotation.x, -90.0f, 90.0f);
        // rotate camera
        cam.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0.0f);
        // rotate player
        tr.rotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);
        // move in the direction of the camera horizontally same speed no matter the angle
        Vector3 lookDir = new Vector3(tr.forward.x, 0.0f, tr.forward.z).normalized;
        Vector3 moveDir = (lookDir * v + tr.right * h).normalized;
        rb.AddForce(moveDir * speed * 200 * Time.deltaTime, ForceMode.Force);

        // Combat
        // fire bullet on button press
        if(Input.GetKey(KeyCode.E)){
            // rate of fire check
            if (timeToFire >= rateOfFire) {
                timeToFire = 0.0f;
                // use raycast to find target
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                Vector3 target;
                if (Physics.Raycast(ray, out hit)) {
                    target = hit.point;
                } else {
                    target = ray.GetPoint(100);// max range
                }
                // get direction to target
                Vector3 targetDir = target - tr.position; 
                // spawn bullet
                Vector3 spawnPos = tr.position + tr.forward * 2.0f;
                GameObject currentBullet = Instantiate(bullet, spawnPos, Quaternion.identity);
                currentBullet.transform.forward = targetDir.normalized;
                // fire bullet
                currentBullet.GetComponent<Rigidbody>().AddForce(targetDir.normalized * 50.0f, ForceMode.Impulse);
                // destroy bullet after 2 seconds
                Destroy(currentBullet, 2.0f);
            }
        }
        // increment time to fire
        timeToFire += Time.deltaTime;
    }
    // called by zombies when they hit the player
    public void TakeDamage(int damage) {
        health -= damage;
    }
    // called by zombies when they die
    public void IncrementScore() {
        score++;
    }
}
