// Source: CIS 376 EnemyController.cs
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    //enum State { WANDERING, STOPPED, SNIFFING, CHASING, WOUNDED, RETREATING }
    enum State { WANDERING, STOPPED, CHASING, SWINGING}
    //[SerializeField] private GameObject[] locations;
    [SerializeField] private int health = 100;
    
    private NavMeshAgent nav;
    private Animator animator;
    private GameObject player;
    //private int currentLocation = 0;
    private State state = State.WANDERING; // Default state, change to sleep later

    private Rigidbody rb;

    //ScriptName attackScript = player.GetComponent<TakeDamage>();
    public PlayerController playerController;
    private float attackRate = 1.0f; // Time between swings
    private float swingTimer = 1.0f; // Timer for swing delay (start at 1 to swing immediately)

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");// OBJECT NAME TO LOOK FOR, REQ name for player
        // Targets our bear will walk between.
        // locations = new GameObject[] // TOCONSIDER: do we want to use this approach?
        // {
        //     GameObject.Find("TargetOne"),
        //     GameObject.Find("TargetTwo"),
        //     GameObject.Find("TargetThree")
        // };
        // // Set first target.
        // nav.SetDestination(locations[0].transform.position);
    }

    void Update()
    {
        // Get distance to player
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Less than 20?  We can see them.  Go get them~
        if (distance < 3f) {
            StartSwinging();
        }
        else if (distance < 20f) {
            StartChasing();//change later
        }
        // else if (distance < 30f) {
        //     // Less than 30 but over 20?  We sense something...
        //     StartStopped();
        // }
        else if (distance > 50f) {
            // No detection.  Keep going.
            StartWandering();
        }
        // // Near an objective?  Get the next one.
        // if (state == State.WANDERING && nav.remainingDistance < 0.6f){
        //     nav.SetDestination(locations[++currentLocation % locations.Length].transform.position);
        // }
        // // If we sense someone, sniff for more info.
        // if (state == State.STOPPED) {
        //     StartCoroutine(Sniff());
        // }
        // Go after them!
        if (state == State.CHASING) {
            UpdateChasing();
        } else if (state == State.SWINGING) {
            UpdateSwinging();
        }
    }
    // swing delay
    private IEnumerator SwingDelay()
    {
        yield return new WaitForSeconds(0.5f);
        // Deal damage to player
        //playerController.TakeDamage(10);
        //SetState(State.WANDERING, "Combat Idle");
    }
    void UpdateSwinging()
    {
        //animator.Play("Attack");
        //float animationLength = 0.5f; // Set this to the length of your attack animation
        //sleep for half a second then deal damage
        //yield return new WaitForSeconds(animationLength);
        //playerController.TakeDamage(10);
        if (swingTimer >= attackRate) {
            swingTimer = swingTimer - attackRate; // reset the timer
            // Deal damage to player
            playerController.TakeDamage(10);
        }
        swingTimer += Time.deltaTime;
    }
    void StartSwinging()
    {
        if (state is State.SWINGING) return;
        SetState(State.SWINGING, "Attack");
        // swing on first contact
        playerController.TakeDamage(10);
        //animator.Play("Attack");
        //float animationLength = 0.5f; // Set this to the length of your attack animation
        //sleep for half a second then deal damage
    }

    void StartChasing()
    {
        //if (state is State.CHASING or State.WOUNDED or State.RETREATING) return;
        if (state is State.CHASING ) return;
        SetState(State.CHASING, "RunForward");
    }

    void StartStopped()
    {
        //if (state is State.SNIFFING or State.CHASING or State.WOUNDED or State.RETREATING) return;
        if (state is State.CHASING) return;
        SetState(State.STOPPED);
    }

    void StartWandering()
    {
        SetState(State.WANDERING, "Combat Idle");
        nav.speed = 3.5f;
        //nav.SetDestination(locations[currentLocation].transform.position);
    }

    void UpdateChasing()
    {
        nav.isStopped = false;
        nav.SetDestination(player.transform.position);
        nav.speed = 7f;
    }

    // void StartRetreating()
    // {
    //     SetState(State.RETREATING);
    //     nav.SetDestination(Random.insideUnitCircle * 100f);
    // }

    void SetState(State newState, string animation = null)
    {
        state = newState;
        //if (animation != null) animator.Play(animation); //uncomment once we have animations
        Debug.Log(state);
    }

    void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.CompareTag("Bullet"))
        // {
        //     health -= 100;
        //     //if (health < 300) SetState(State.WOUNDED);
        // }
    }
}
