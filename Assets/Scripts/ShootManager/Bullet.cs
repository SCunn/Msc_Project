
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
    // private bool hasHit = false;
    // For enemy damage
    [SerializeField] int enemyDamage;

    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, bulletLifeSpan);
    }
    // Method to move the bullet
    void FixedUpdate()
    {
        // Move the rigidbody forward
        if (speed != 0 && rb != null) rb.position += (transform.forward) * (speed * Time.deltaTime);
    }


    // private void OnCollisionEnter(Collision collision)
    // {

    //      if (collision.gameObject.tag != "Bullet" && !hasHit) 
    //      {
    //          hasHit = true;
    //          speed = 0;
    //          Destroy(gameObject);
    //      }

    //      if (collision.gameObject.tag == "Enemy")
    //      {
    //         //  Destroy(collision.gameObject);
    //         collision.gameObject.GetComponent<Enemy>().DamageEnemy(enemyDamage);
    //      } 
    // }



    // private void OnTriggerEnter(Collider other)
    // {
    //     // if (other.gameObject.tag == "Enemy")
    //     // {
    //         other.gameObject.GetComponent<Enemy>().DamageEnemy(enemyDamage);
    //         // Destroy(gameObject);
    //     // }
    // }

    // private void OnTriggerStay(Collider other)
    // {
    //     // if (other.gameObject.tag == "Enemy")
    //     // {
    //         other.gameObject.GetComponent<Enemy>().DamageEnemy(enemyDamage);
    //     // }
    // }

}
