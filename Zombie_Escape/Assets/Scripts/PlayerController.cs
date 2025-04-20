// Reference: https://www.reddit.com/r/Unity3D/comments/8k7w7v/unity_simple_mouselook/
// Reference: https://discussions.unity.com/t/how-do-i-move-a-camera-with-mouse/194032/2
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform tr;
    public GameObject player;
    public float sensitivity = 10.0f;
    Vector2 rotation = Vector2.zero;
    Vector2 deltaRot = Vector2.zero;
    public Camera cam;
    public Rigidbody rb;
    public float speed = 20.0f;
    [SerializeField] private int health = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tr = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // check hp to see if we lose
        if (health <= 0) {
            // lose
            Debug.Log("You lose!");
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // ^^^ these are -1 to 1 with the WASD keys
        
        // if(Input.GetKey(KeyCode.Q)){
        //     //tr.Rotate(0.0f, -1.0f, 0.0f, Space.Self);
        //     tr.Rotate(new Vector3(0.0f, -1.0f, 0.0f) * Time.deltaTime * 100.0f, Space.Self);
        // }
        // if(Input.GetKey(KeyCode.E)){
        //     //tr.Rotate(0.0f, 1.0f, 0.0f, Space.Self);
        //     tr.Rotate(new Vector3(0.0f, 1.0f, 0.0f) * Time.deltaTime * 100.0f, Space.Self);
        // }
        
        // mouse input
        // deltaRot.x = Input.GetAxis("Mouse X") * sensitivity;
        // deltaRot.y = Input.GetAxis("Mouse Y") * sensitivity;
        rotation.x += Input.GetAxis("Mouse X") * sensitivity;
        rotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        //rotation.x += deltaRot.x;
        //rotation.y -= deltaRot.y;
        // clamp vertical rotation
        rotation.y = Mathf.Clamp(rotation.y, -90.0f, 90.0f);

        // position only moves horizontally
        //Vector3 deltaPos = new Vector3();
        //Vector3 deltaPos = new Vector3(tr.forward.x, 0.0f, tr.forward.z);

        //normalized movement vector
        // movement vector moves at same rate in all directions horizontally
        Vector3 deltaPos = Quaternion.Euler(0.0f, rotation.x, 0.0f) * new Vector3(h, 0.0f, v);
        //Vector3 deltaPos = tr.rotation.y*new Vector3(h, 0.0f, v);
        // move forward/backward at same rate no matter vertical angle
        
            //deltaPos.y = 0.0f; // no vertical movement
        //new position

        // tr.SetPositionAndRotation(
        //     tr.position + (deltaPos * speed * Time.deltaTime),
        //     Quaternion.Euler(rotation.y, rotation.x, 0.0f)
        // );
        
        //Vector3 rot = new Vector3(rotation.y, rotation.x, 0.0f);
        //Vector3 rot = new Vector3(-deltaRot.y, deltaRot.x, 0.0f);
        //tr.Rotate(rot);

        rb.MoveRotation(Quaternion.Euler(rotation.y, rotation.x, 0.0f));
        rb.MovePosition(rb.position + (deltaPos * speed * Time.deltaTime));
    }

    public void TakeDamage(int damage) {
        health -= damage;
        Debug.Log("Player took damage: " + damage);
        Debug.Log("Player health: " + health);
    }
    
}
