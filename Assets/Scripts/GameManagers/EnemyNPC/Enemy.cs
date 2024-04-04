using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // // Player Info
    PlayerHealth playerHP;
    // GameObject player;

    // Enemy Stats and Info
    [SerializeField] float damage = 10;
    [SerializeField] float enemyHP = 100;
    [SerializeField] float knockback = 10;
    [SerializeField] float knockbackMutiplier = 5;
    EnemySpawner Spawner;

    // Start is called before the first frame update
    void Start()
    {
        playerHP = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerHealth>(); // find the player gameObject and set it to the player variable, more fore dynamic dificulty scaling
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
    //     if (playerHealth)
    //     {
    //         DamagePlayer();
    //     }

    // }

    // void DamagePlayer()
    // {
    //     playerHP.Damage(damage);
    // }

    void KillEnemy()
    {
        if (Spawner != null) Spawner.currentSpawnedEnemies.Remove(this.gameObject);
        Destroy(gameObject);
    }

    public void SetSpawner(EnemySpawner _spawner)
    {
        Spawner = _spawner;
    }
}
