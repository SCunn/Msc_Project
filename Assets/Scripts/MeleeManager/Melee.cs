using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    // Velovity of the axe collider
    // public float axeVelocity = 0;

    // enemy damage
    public int enemyDamage = 20;
    // Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // rb = GetComponent<Rigidbody>();
    }

    // On trigger enter, if the object is an enemy and check axe collider velocity, damage the enemy
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" /*&& GetComponent<Rigidbody>().velocity.magnitude > axeVelocity*/)
        {
            other.gameObject.GetComponent<Enemy>().DamageEnemy(enemyDamage);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().DamageEnemy(0);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().DamageEnemy(0);
        }
    }
}
