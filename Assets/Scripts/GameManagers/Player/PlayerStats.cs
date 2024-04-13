using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
// using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int maxHP = 100;
    public int HP;
    // public OVRCameraRig cameraRig; // Reference to the OVRCameraRig
    // public Image healthbar; // Reference to the health bar image
    

    // Start is called before the first frame update
    void Start()
    {
       HP = maxHP; 
    }

    void Update()
    {
        // healthbar.fillAmount = HP / maxHP;
    }

    public int GetHP()
    {
        return HP;
    }

    public void Damage(int Damage)
    {
        HP -= Damage;
        if(HP <= 0)
        {
            HP = 0;
            // // Handle player death (e.g., disable movement, play death animation)
            // cameraRig.enabled = false; // Disable camera rig on death (optional)
            // Debug.Log("Player is dead");

            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        // Debug.Log("Player HP: " + HP);
    }

    // public void Heal(float healAmount)
    // {
    //     HP = Mathf.Clamp(HP + healAmount, 0f, maxHP);
    // }
}
