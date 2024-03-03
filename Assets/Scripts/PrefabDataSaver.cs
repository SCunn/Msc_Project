using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.XR;

public class PrefabDataSaver : MonoBehaviour
{
    public static PrefabDataSaver Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    public void SavePrefabToAnchor(GameObject prefab, OVRSpatialAnchor anchor)
    {
        if (prefab == null || anchor == null) return;

        string uniqueKey =$"{anchor.name}_{Guid.NewGuid()}";
        PlayerPrefs.SetString(uniqueKey, prefab.name);

        // // Alternative approach for data storage:
        // string jsonData = JsonUtility.ToJson(new PrefabData
        // {
        //     PrefabName = prefab.name,
        //     Position = prefab.transform.position,
        //     Rotation = prefab.transform.rotation,
        //     // Add other data as needed
        // });

        // PlayerPrefs.SetString(uniqueKey, jsonData);

        Debug.Log("Prefab data saved for anchor " + anchor.name + " with key: " + uniqueKey);
    }

    public string LoadPrefabNameFromAnchor(OVRSpatialAnchor anchor)
    {
        string uniqueKey = $"{anchor.name}_{Guid.NewGuid()}"; 
        if (PlayerPrefs.HasKey(uniqueKey))
        {
            return PlayerPrefs.GetString(uniqueKey);
        }
        else
        {
            return null; 
        }
    }

    // public class PrefabData
    // {
    //     public string PrefabName { get; set; }
    //     public Vector3 Position { get; set; }
    //     public Quaternion Rotation { get; set; }
    //     // Add other properties as needed
    // }
}