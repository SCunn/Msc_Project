// Purpose: This script is used to play the footstep sounds of the enemy NPCs. It also determines the type of ground the enemy is walking on and plays the appropriate fotstep sound. Script originally sourced from SwishSwoosh, https://www.youtube.com/watch?v=xbtSM7B_htU&list=PLBJRkPI3sMotWi_4fBJg-HubHga4FGi44&index=3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioClip[] attackSounds;

    // Wolf Death Sound
    public AudioClip wolfDeathSound;
    
    [Header("Footsteps")]
    public List<AudioClip> earthFS;
    public List<AudioClip> woodFS;

    enum GroundType
    {
        Earth,
        Wood, 
        Empty
    }

    public AudioSource footstepSource, attackSource, deathSource;

    // Start is called before the first frame update
    void Start()
    {
        // footstepSource = GetComponent<AudioSource>();
        footstepSource = GetComponents<AudioSource>()[0];  // eg. This is the first audio source in the array
        attackSource = GetComponents<AudioSource>()[1];    // eg. This is the second audio source in the array
        deathSource = GetComponents<AudioSource>()[2];
        
    }

    void AttackSound()
    {
        AudioClip clip = attackSounds[(int)Random.Range(0, attackSounds.Length)];
        attackSource.clip = clip;
        attackSource.Play();
    }

    void DeathSound()
    {
        deathSource.clip = wolfDeathSound;
        deathSource.Play();
    }

    private GroundType SurfaceSelect()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, -Vector3.up);  // Raycast from the center of the object
        Material surfaceMaterial;

        if (Physics.Raycast(ray, out hit, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Renderer surfaceRenderer = hit.collider.GetComponent<Renderer>();
            if (surfaceRenderer)
            {
                surfaceMaterial = surfaceRenderer ? surfaceRenderer.sharedMaterial : null;
                if (surfaceMaterial.name.Contains("Floor"))
                {
                    return GroundType.Earth;
                }
                else if (surfaceMaterial.name.Contains("wood"))
                {
                    return GroundType.Wood;
                }
                else
                {
                    return GroundType.Empty;
                }
            }
        }
        return GroundType.Empty;
    }

    public void PlayFootstep()
    {
        AudioClip clip = null;

        GroundType surface = SurfaceSelect();

        switch (surface)
        {
            case GroundType.Earth:
                clip = earthFS[Random.Range(0, earthFS.Count)];
                break;
            case GroundType.Wood:
                clip = woodFS[Random.Range(0, woodFS.Count)];
                break;
            default:
                break;
        }

        if (surface != GroundType.Empty)
        {
            footstepSource.clip = clip;
            footstepSource.volume = Random.Range(/*0.02f, 0.05f*/0.6f, 1f);
            footstepSource.pitch = Random.Range(0.8f, 1.2f);
            footstepSource.PlayOneShot(clip);
        }
    }
}
