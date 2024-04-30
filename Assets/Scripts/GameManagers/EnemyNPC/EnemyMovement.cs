// Purpose: Controls the movement of the enemy
// original script from Big Bird Games, https://www.youtube.com/watch?v=SXyLO3q8DD0&ab_channel=BigBirdGames and The Game Dev Cave, https://www.youtube.com/watch?v=DnIBoUBezeU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;        
using UnityEngine.AI;
// using Oculus.VR;

public class EnemyMovement : MonoBehaviour
{
    //// variables
    // Original code
    private Transform player; // reference to the player, this allows for the enemy to follow the player based on the player's transfom position

    // GameObject player; // reference to the player, this allows for the enemy to follow the player based on the player's transfom position

    private NavMeshAgent agent; // reference to the nav mesh agent, this allows for the enemy to move around the scene

    // public float enemyDistance = 0.7f; // Distance enemy will have to be from player to play animation
    bool hurdling; // boolean to check if the enemy is hurdling
    // OffMeshLinkData 
    // private OffMeshLinkData _currLinkData;

    [SerializeField] LayerMask groundLayer, playerLayer; // LayerMask to determine what is ground and what is player

    Animator animator; // reference to the animator, this allows for the enemy to play animations

    BoxCollider boxCollider; // reference to the box collider placed on enemy attack area

    // private bool isDead = false; // boolean to check if the enemy is dead   

    // Patrolling
    Vector3 destPoint; // destination point for the enemy to move to
    bool walkpointSet;
    [SerializeField] float range;

    // // Attacking
    // float timeBetweenAttacks; // time between attacks
    // float attackTimer; // timer for the attack
    // bool canAttack; // boolean to check if the enemy can attack



    // state change
    [SerializeField] float sightRange, attackRange; // range for the enemy to see the player and attack the player
    bool playerInSight, playerInAttackRange; // boolean to check if the player is in the sight range and attack range


    // Spawner
    //public delegate void EnemyKilled();      // delegate is a type that represents references to methods with a particular parameter list and return type
    //public static event EnemyKilled OnEnemyKilled;       // event is a keyword that enables a class or object to provide notifications

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // get the NavMeshAgent component from the enemy gameObject
        player = GameObject.FindGameObjectWithTag("MainCamera").transform; // find the player gameObject and set it to the player variable
        animator = GetComponent<Animator>(); // get the Animator component
        boxCollider = GetComponentInChildren<BoxCollider>(); // get the BoxCollider component
    }

    // Update is called once per frame
    void Update()
    {
        // transform.LookAt(player); // make the enemy look at the player
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // make the enemy rotate only on the y axis

        // agent.SetDestination(player.position); // set the destination of the enemy to the player's position

        // // if the distance between the enemy and the player is less than or equal to the enemyDistance.
        // if (Vector3.Distance(transform.position, player.position) <= enemyDistance) 
        // {
        //     gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;    // set the velocity of the enemy to zero, so it stops moving
            
        //     gameObject.GetComponent<Animator>().Play("attack2");     // play the attack1 animation
        // } 

    
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer); // check if the player is in the sight range
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer); // check if the player is in the attack range

        if (!playerInSight && !playerInAttackRange) Patrol();
        if (playerInSight && !playerInAttackRange) Chase();
        if (playerInSight && playerInAttackRange) Attack();

        // OffMeshLink Vault Detection logic, code originally sourced from Russel Canfield, https://www.youtube.com/watch?v=Yz1Y6qhHoq8&t=473s 
        if (agent.isOnOffMeshLink)
            {
                var meshLink = agent.currentOffMeshLinkData;

                if (!hurdling && meshLink.offMeshLink.area == NavMesh.GetAreaFromName("Hurdle"))
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("VaultRoot"))
                    {
                        animator.SetTrigger("Vault");
                        // agent.SetDestination(transform.position);
                        // set agent destination to stop agent from jumping back at the end of the vault animation
                        // agent.SetDestination(agent.destination);
                        // _currLinkData = meshLink;
                        hurdling = true;

                    }

                }
            }
            else
            {
                hurdling = false;
            } 


        

    }

    void Chase()
    {
        agent.SetDestination(player.transform.position);
        // Debug.Log("Chasing Player");
    }

    void Attack()
    {

        transform.LookAt(player); // make the enemy look at the player
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // make the enemy rotate only on the y axis
        
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
        {
            animator.SetTrigger("Attack");
            agent.SetDestination(transform.position);
            // Debug.Log("Attacking Player");
        }
        
    }

    void Patrol()
    {
        if (!walkpointSet) SearchForDest();
        if (walkpointSet) agent.SetDestination(destPoint);
        if (Vector3.Distance(transform.position, destPoint) < 10) walkpointSet = false;
        // Debug.Log("Patrolling");
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destPoint, Vector3.down, 5f, groundLayer))
        {
            walkpointSet = true;
        }
    }
        
    void EnableAttack()
    {
        boxCollider.enabled = true;
    }

    void DisableAttack()
    {
        boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            // Destroy(other.gameObject);
            // Destroy(gameObject);
            // OnEnemyKilled();
            print("Hit!!!!!!!!!");
        }
    }

    // void DetectOffMeshLink()
    // {
    //     if (agent.enabled)
    //     {
    //         if (agent.isOnOffMeshLink)
    //         {
    //             var meshLink = agent.currentOffMeshLinkData;

    //             if (!hurdling && meshLink.offMeshLink.area == NavMesh.GetAreaFromName("Hurdle"))
    //             {
    //                 if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Vault"))
    //                 {
    //                     animator.SetTrigger("Vault");
    //                     agent.SetDestination(transform.position);
    //                     hurdling = true;
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             hurdling = false;
    //         } 
    //     }
        
    // }

}
