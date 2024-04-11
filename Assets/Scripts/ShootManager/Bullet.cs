
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //////// Variables ////////
    // Enemy Info
    // Enemy enemyHP;
    
    // Speed of the bullet
    public float speed = 20f;
    // Fire rate of the bullet
    public float fireRate = 1f;
    // Bullet lifespan in seconds
    private float bulletLifeSpan = 3f;
    // The Rigidbody of the bullet
    private Rigidbody rb = null;
    // Collision Checker
    private bool hasHit = false;
    // For enemy damage
    [SerializeField] int enemyDamage;

    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, bulletLifeSpan);
        
        // // Find the enemy object and get the Enemy component
        // GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
        // if (enemyObject != null)
        // {
        //     enemyHP = enemyObject.GetComponent<Enemy>();
        // }
        // else
        // {
        //     print("Cannot find enemy object.");
        //     return;
        //     // Debug.LogError("Cannot find enemy object.");
        // }
    }
    // Method to move the bullet
    void FixedUpdate()
    {
        // Move the rigidbody forward
        if (speed != 0 && rb != null) rb.position += (transform.forward) * (speed * Time.deltaTime);
    }

    // // Method to check for collision
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.tag != "Bullet" && !hasHit) 
    //     {
    //         hasHit = true;

    //         speed = 0;

    //         Destroy(gameObject);
    //     }
    //     if (collision.gameObject.tag == "Enemy")
    //     {
    //         Destroy(collision.gameObject);
            
    //     }      
    // }


    // private void OnTriggerEnter(Collider other)
    // {
    //     var enemy = other.gameObject.GetComponent<Enemy>();
    //     if (enemy != null)
    //     {
    //         enemy.DamageEnemy(Damage, Player.transform);
    //     }
    // }

    private void OnCollisionEnter(Collision collision)
    {

         if (collision.gameObject.tag != "Bullet" && !hasHit) 
         {
             hasHit = true;
             speed = 0;
             Destroy(gameObject);
         }

         if (collision.gameObject.tag == "Enemy")
         {
            // Enemy target = collision.gameObject.GetComponent<Enemy>();
            // enemyHP.DamageEnemy(enemyDamage);
            // Debug.Log("Hit Enemy"+ enemyDamage);
            //  Destroy(collision.gameObject);
            collision.gameObject.GetComponent<Enemy>().DamageEnemy(enemyDamage);
         } 
    }

}
