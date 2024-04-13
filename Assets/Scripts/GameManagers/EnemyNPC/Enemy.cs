using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    // // Player Info
    PlayerStats playerHP;
    // Referennce to Animator
    Animator animator;

    // Enemy Stats and Info
    [SerializeField] int damage = 10;
    [SerializeField] int enemyHP = 100;
    // [SerializeField] float knockback = 10;
    // [SerializeField] float knockbackMutiplier = 5;


    EnemySpawner Spawner;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerHP = GameObject.Find("CenterEyeAnchor").GetComponent<PlayerStats>(); // find the player gameObject and set it to the player variable, more fore dynamic dificulty scaling     
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var playerHealth = collision.gameObject.GetComponent<PlayerStats>();
        if (playerHealth)
        {
            DamagePlayer();
        }

    }

    void DamagePlayer()
    {
        playerHP.Damage(damage);
    }

    public void DamageEnemy(int enemyDamage/*, Transform player*/)
    {
        enemyHP -= enemyDamage;
        
        // if Animator object is not null, set the trigger to Damage
        if (animator != null) 
        {
            // add damage animation
            if ( !animator.GetCurrentAnimatorStateInfo(0).IsName("damage"))
            {
                animator.SetTrigger("Damage");
            }
        }

        if (enemyHP <= 0)
        {
            enemyHP = 0;
            // Destroy(gameObject);
            KillEnemy();
            // return;
        }
        // Debug.Log("Enemy HP: " + enemyHP);
        
        // rb.velocity = Vector3.up * 5;
        // rb.AddForce(player.forward * 5);


        // else 
        // {
        //     // Vector3 direction = transform.position - player.position;
        //     // direction.y = 0;
        //     // GetComponent<NavMeshAgent>().velocity = direction.normalized * knockback * knockbackMutiplier;

        // }
    }

    void KillEnemy()
    {  

        if (Spawner != null) 
        {
            Spawner.currentSpawnedEnemies.Remove(this.gameObject);
        }
        else
        {
            return;
            // Debug.LogError("Spawner is null");
        }
        

        // add death animation
        if ( !animator.GetCurrentAnimatorStateInfo(0).IsName("die"))
        {
            // Stop the enemy from Chase() Patrol() Attack()
            GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            GetComponent<NavMeshAgent>().isStopped = true;
            // GetComponent<NavMeshAgent>().enabled = false;

            animator.SetTrigger("Death");
            Destroy(gameObject, 2.5f);
        }

        // Destroy(gameObject);
    }

    public void SetSpawner(EnemySpawner _spawner)
    {
        Spawner = _spawner;
    }
}
