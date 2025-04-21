// Reference: https://www.reddit.com/r/Unity3D/comments/8k7w7v/unity_simple_mouselook/
// Reference: https://discussions.unity.com/t/how-do-i-move-a-camera-with-mouse/194032/2
// Reference: https://www.youtube.com/watch?v=f473C43s8nE
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float speed = 2.0f;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    
    Transform tr;
    public GameObject endMenu;
    public GameObject player;
    public GameObject bullet;
    public float sensitivity;
    Vector2 rotation = Vector2.zero;
    Vector2 deltaRot = Vector2.zero;
    public Camera cam;
    public Rigidbody rb;
    private bool running = true;
    

    private int score = 0;
    private int maxScore = 10;
    [SerializeField] private int health = 100;

    private float rateOfFire = 0.5f;
    private float timeToFire = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;// how to modify
        tr = gameObject.transform;
        Transform camTransform = cam.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!running) {
            return;
        }
        // check score to see if we win
        if (score >= maxScore) {
            // win
            // bring up win screen
            Debug.Log("You win!");
            Time.timeScale = 0.0f;
            endMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            running = false;
        }
        // check hp to see if we lose
        if (health <= 0) {
            // lose
            // bring up death screen
            Debug.Log("You lose!");
            Time.timeScale = 0.0f;
            endMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            running = false;
        }
        // check if grounded
        grounded = Physics.Raycast(tr.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (grounded) {
            //rb.drag = groundDrag;
            rb.linearDamping = groundDrag;
        }
        else {
            //rb.drag = 0;
            rb.linearDamping = 0;
        }

        // speed cap
        Vector3 velocity = new Vector3(rb.linearVelocity.x, 0.0f, rb.linearVelocity.z);
        if (velocity.magnitude > speed) {
            Vector3 newVelocity = velocity.normalized * speed;
            rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        //float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity;
        // ^^^ these are -1 to 1 with the WASD keys
        
        // // mouse input
        rotation.y += mouseX;
        rotation.x -= mouseY;
        // clamp vertical rotation
        rotation.x = Mathf.Clamp(rotation.x, -90.0f, 90.0f);

        // rotate camera
        cam.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0.0f);
        // rotate player
        tr.rotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);

        // move in the direction of the camera horizontally same speed no matter the angle
        //need delta time
        Vector3 lookDir = new Vector3(tr.forward.x, 0.0f, tr.forward.z).normalized;
        Vector3 moveDir = (lookDir * v + tr.right * h).normalized;
        //Vector3 moveDir = (tr.forward * v + tr.right * h).normalized;
        rb.AddForce(moveDir * speed * 200 * Time.deltaTime, ForceMode.Force);

        // fire bullet on button press
        if(Input.GetKey(KeyCode.E)){
            if (timeToFire >= rateOfFire) {
                //timeToFire = timeToFire - rateOfFire;
                timeToFire = 0.0f;

                //tr.Rotate(0.0f, 1.0f, 0.0f, Space.Self);
                //fire bullet
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                Vector3 target;
                if (Physics.Raycast(ray, out hit)) {
                    target = hit.point;
                } else {
                    target = ray.GetPoint(100);// max range
                }
                Vector3 targetDir = target - tr.position; 

                // spawn bullet
                Vector3 spawnPos = tr.position + tr.forward * 2.0f;
                GameObject currentBullet = Instantiate(bullet, spawnPos, Quaternion.identity);
                currentBullet.transform.forward = targetDir.normalized;
                currentBullet.GetComponent<Rigidbody>().AddForce(targetDir.normalized * 50.0f, ForceMode.Impulse);
                // destroy bullet after 2 seconds
                Destroy(currentBullet, 2.0f);
                Debug.Log("Bullet fired");
            }
            
        }
        timeToFire += Time.deltaTime;
    }

    public void TakeDamage(int damage) {
        health -= damage;
        Debug.Log("Player took damage: " + damage);
        Debug.Log("Player health: " + health);
    }
    public void IncrementScore() {
        score++;
        Debug.Log("Player score: " + score);
    }
    
}
