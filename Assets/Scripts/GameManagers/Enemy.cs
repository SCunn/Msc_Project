using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // // Player Info
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
