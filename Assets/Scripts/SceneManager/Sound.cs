// Purpose: This script is used to store the sound information for the sound manager. It contains the name of the sound, the clip, the volume, and the pitch.  Original code sourced from Brackeys, https://www.youtube.com/watch?v=6OT43pvUyfY 
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    // [Range(0f, 1f)]
    // public float stereopan;

    [Range(0f, 1f)]
    public float spatialBlend;

    // [Range(0f, 1.1f)]
    // public float reverbZoneMix;



    [HideInInspector]
    public AudioSource source;
}
