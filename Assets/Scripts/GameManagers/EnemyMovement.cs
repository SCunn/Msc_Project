// Purpose: Controls the movement of the enemy
// original script from Big Bird Games, https://www.youtube.com/watch?v=SXyLO3q8DD0&ab_channel=BigBirdGames
using System.Collections;
using System.Collections.Generic;
using UnityEngine;        
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    //// variables

    private Transform player; // reference to the player, this allows for the enemy to follow the player based on the player's transfom position

    private NavMeshAgent agent; // reference to the nav mesh agent, this allows for the enemy to move around the scene

    public float enemyDistance = 0.7f; // Distance enemy will have to be from player to play animation

    // Spawner
    //public delegate void EnemyKilled();      // delegate is a type that represents references to methods with a particular parameter list and return type
    //public static event EnemyKilled OnEnemyKilled;       // event is a keyword that enables a class or object to provide notifications

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // find the player gameObject and set it to the player variable
        agent = GetComponent<NavMeshAgent>(); // get the NavMeshAgent component from the enemy gameObject
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player); // make the enemy look at the player
        agent.SetDestination(player.position); // set the destination of the enemy to the player's position

        // if the distance between the enemy and the player is less than or equal to the enemyDistance.
        if (Vector3.Distance(transform.position, player.position) <= enemyDistance) 
        {
            gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;    // set the velocity of the enemy to zero, so it stops moving
            // gameObject.GetComponent<Animator>().Play("attack");                 // play the attack animation
        } 

    }

}
