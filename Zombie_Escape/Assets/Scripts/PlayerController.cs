// Reference: https://www.reddit.com/r/Unity3D/comments/8k7w7v/unity_simple_mouselook/
// Reference: https://discussions.unity.com/t/how-do-i-move-a-camera-with-mouse/194032/2
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform tr;
    public GameObject player;
    public float sensitivity = 10.0f;
    Vector2 rotation = Vector2.zero;
    public float speed = 20.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tr = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // if(Input.GetKey(KeyCode.Q)){
        //     //tr.Rotate(0.0f, -1.0f, 0.0f, Space.Self);
        //     tr.Rotate(new Vector3(0.0f, -1.0f, 0.0f) * Time.deltaTime * 100.0f, Space.Self);
        // }
        // if(Input.GetKey(KeyCode.E)){
        //     //tr.Rotate(0.0f, 1.0f, 0.0f, Space.Self);
        //     tr.Rotate(new Vector3(0.0f, 1.0f, 0.0f) * Time.deltaTime * 100.0f, Space.Self);
        // }
        
        // mouse input
        rotation.x += Input.GetAxis("Mouse X") * sensitivity;
        rotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        // clamp vertical rotation
        rotation.y = Mathf.Clamp(rotation.y, -90.0f, 90.0f);

        // position only moves horizontally
        Vector3 deltaPos = new Vector3(h, 0.0f, v);
        
        // Combine rotation and position updates for optimization
        // tr.SetPositionAndRotation(
        //     tr.position + (tr.forward * (0.1f * v * Time.deltaTime * 100.0f)) + (tr.right * (0.1f * h * Time.deltaTime * 100.0f)),
        //     Quaternion.Euler(rotation.y, rotation.x, 0.0f)
        // );
        tr.SetPositionAndRotation(
            tr.position + deltaPos * speed * Time.deltaTime,
            Quaternion.Euler(rotation.y, rotation.x, 0.0f)
        );
    }
    
}
