// Author: Richard Roy
// Date: April 21, 2025
// Source: CIS 376 EnemyController.cs
// Description: Controls the zombie AI behavior and animations
//              Handles states, movement, combat, and death
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    // Player references
    private GameObject player;
    public PlayerController playerController;
    // Zombie references
    private NavMeshAgent nav;
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider hitbox;
    // Zombie variables/state
    enum State { WANDERING, STOPPED, CHASING, SWINGING, DYING}
    private State state = State.WANDERING;
    [SerializeField] private int health = 100;
    private float attackRate = 0.792f; // Time between swings
    private float swingTimer = 0.792f; // Timer for swing delay (start at 1 to swing immediately)
    private bool primaryAttack = true;//alternate between attacks

    void Start()
    {
        // Get references to components
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        hitbox = GetComponent<CapsuleCollider>();
        player = GameObject.Find("Player");// OBJECT NAME TO LOOK FOR, REQ name for player
        playerController = player.GetComponent<PlayerController>();
    }
    void Update()
    {
        // Check if zombie is dead
        if (health <= 0) {
            StartDying();
            return;
        }
        // Get distance to player
        float distance = Vector3.Distance(transform.position, player.transform.position);
        // melee range
        if (distance < 3f) {
            StartSwinging();
        }
        // aggro range
        else if (distance < 20f) {
            StartChasing();
        }
        // lose aggro range
        else if (distance > 50f) {
            StartWandering();
        }
        // State updates
        if (state == State.CHASING) {
            UpdateChasing();
        } else if (state == State.SWINGING) {
            UpdateSwinging();
        } else if (state == State.WANDERING) {
            UpdateWandering();
        }
    }
    // Update for attacking
    void UpdateSwinging()
    {
        // Only swing if swing timer is up
        if (swingTimer >= attackRate) {
            swingTimer = swingTimer - attackRate; // reset the timer
            // alternate between attack animations
            if (primaryAttack) {
                animator.Play("Attack1");
                primaryAttack = false;
            } else {
                animator.Play("Attack2");
                primaryAttack = true;
            }
            // player takes damage
            playerController.TakeDamage(10);
        }
        // keep track of the swing timer
        swingTimer += Time.deltaTime;
    }
    void StartSwinging()
    {
        if (state is State.SWINGING) return;
        SetState(State.SWINGING, "Attack");
        animator.Play("Attack1");
    }

    void StartChasing()
    {
        if (state is State.CHASING ) return;
        SetState(State.CHASING, "RunForward");
        animator.Play("WalkFastFWD");
    }
    void StartWandering()
    {
        SetState(State.WANDERING, "Combat Idle");
        nav.speed = 0f;
        animator.Play("Idle");
    }
    void StartDying()
    {
        if (state is State.DYING) return;
        nav.speed = 0f;
        //disable hitbox to prevent blocking of bullets
        hitbox.enabled = false;
        // player gets a point for killing the zombie
        playerController.IncrementScore();
        SetState(State.DYING, "Death");
        animator.Play("Death");
        Destroy(gameObject, 2f); // Destroy the zombie after 2 seconds
    }    
    void UpdateWandering()
    {
        animator.Play("Idle");
    }
    void UpdateChasing()
    {
        animator.Play("WalkFastFWD");
        nav.isStopped = false;
        nav.SetDestination(player.transform.position);
        nav.speed = 5f;
    }
    void SetState(State newState, string animation=null)
    {
        state = newState;
    }
    // handles zombie getting shot
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health -= 50;
        }
    }
}
