using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SpatialAnchorManager : MonoBehaviour
{
    // [NonSerialized]
    // public OVRSpatialAnchor anchorPrefab;
    public GameObject[] anchorPrefabs; // Array of prefabs
    private int currentPrefabIndex = 0; // Index of the current prefab to spawn
    public const string NumUuidsPlayerPref = "numUuids"; // Key for the number of UUIDs in PlayerPrefs

    private Canvas canvas;
    private TextMeshProUGUI uuidText;
    private TextMeshProUGUI savedStatusText;
    private List<OVRSpatialAnchor> anchors = new List<OVRSpatialAnchor>();

    private OVRSpatialAnchor lastCreatedAnchor;
    private AnchorLoader anchorLoader;

    private void Awake() => anchorLoader = GetComponent<AnchorLoader>();

    // Update is called once per frame
    void Update()
    {
        // Create a spatial anchor when the primary index trigger is pressed
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            CreateSpatialAnchor();
        }

        // Cycle through the array of spatial anchors you want to create
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            currentPrefabIndex = (currentPrefabIndex + 1) % anchorPrefabs.Length;
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            currentPrefabIndex = (currentPrefabIndex - 1 + anchorPrefabs.Length) % anchorPrefabs.Length;
        }

        // Save the last created anchor when the One button is pressed
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            SaveLastCreatedAnchor();
        }

        // Unsave the last created anchor when the Two button is pressed
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            UnsaveLastCreatedAnchor();
        }

        // Unsave all anchors when the primary hand trigger is pressed
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            UnsaveAllAnchors();
        }

        // Load all saved anchors when the primary R thumbstick button is pressed
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
        {
            LoadSavedAnchors();
        }
        
    }

    // public void CreateSpatialAnchor()
    // {
    //     // Instantiate the selected prefab at the controller position
    //     GameObject prefab = Instantiate(anchorPrefabs[currentPrefabIndex], OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
    //         OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));

    //     prefab.AddComponent<OVRSpatialAnchor>();

    //     // Cycle to the next prefab in the array
    //     // currentPrefabIndex = (currentPrefabIndex + 1) % anchorPrefabs.Length;
    // }

    public void CreateSpatialAnchor()
    {
        // Instantiate the selected prefab at the controller position
        GameObject prefab = Instantiate(anchorPrefabs[currentPrefabIndex], OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
            OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));

        OVRSpatialAnchor spatialAnchor = prefab.GetComponent<OVRSpatialAnchor>();
        if (spatialAnchor == null)
        {
            Debug.LogError("OVRSpatialAnchor component not found on the working anchor.");
            return;
        }

        // // Wait until the anchor is created and localized
        // while (!spatialAnchor.Created && !spatialAnchor.Localized)
        // {
        //     return;
        // }

        // // Get the UUID of the anchor and update the UI elements
        // Guid anchorGuid = spatialAnchor.Uuid;
        // Debug.Log(anchorGuid);
        // Debug.Log("UUID: " + anchorGuid.ToString());

        // Get references to the UI elements in the anchor's canvas
        canvas = prefab.gameObject.GetComponentInChildren<Canvas>();
        uuidText = canvas.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        savedStatusText = canvas.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        StartCoroutine(AnchorCreated(spatialAnchor));    
    }

    private IEnumerator AnchorCreated(OVRSpatialAnchor workingAnchor)
    {
        // Wait until the anchor is created and localized
        while (!workingAnchor.Created && !workingAnchor.Localized)
        {
            yield return new WaitForEndOfFrame();
        }

        // Get the UUID of the anchor and update the UI elements
        Guid anchorGuid = workingAnchor.Uuid;
        anchors.Add(workingAnchor);
        lastCreatedAnchor = workingAnchor;

        uuidText.text = "UUID: " + anchorGuid.ToString();
        savedStatusText.text = "Not Saved";
        Debug.Log("UUID: " + anchorGuid.ToString());
    }

    // Save the last created anchor
    private void SaveLastCreatedAnchor()
    {
        // Save the last created anchor and update the saved status text
        if (lastCreatedAnchor != null)
        {
            lastCreatedAnchor.Save((lastCreatedAnchor, success) =>
            {
                if (success)
                {
                    savedStatusText.text = "Saved";
                    Debug.Log("Saved");
                    SaveUuidToPlayerPrefs(lastCreatedAnchor.Uuid);
                }
            });
        } 
        else 
        {
            Debug.Log("Last created anchor is null, cannot save anchor!.");
        }
    }

    void SaveUuidToPlayerPrefs(Guid uuid)
    {
        // Save the UUID to player prefs
        if (!PlayerPrefs.HasKey(NumUuidsPlayerPref))
        {
            PlayerPrefs.SetInt(NumUuidsPlayerPref, 0);
        }
        int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
        PlayerPrefs.SetString("uuid" + playerNumUuids, uuid.ToString());
        PlayerPrefs.SetInt(NumUuidsPlayerPref, ++playerNumUuids);
    }

    private void UnsaveLastCreatedAnchor()
    {
        // Unsave the last created anchor and update the saved status text
        lastCreatedAnchor.Erase((lastCreatedAnchor, success) =>
        {
            if (success)
            {
                savedStatusText.text = "Not Saved";
                Debug.Log("Not Saved");
            }
        });
    }

    private void UnsaveAllAnchors()
    {
        // Unsave all anchors and update the saved status text
        foreach (var anchor in anchors)
        {
            UnsaveAnchor(anchor);
        }
    }

    private void UnsaveAnchor(OVRSpatialAnchor anchor)
    {
        // Unsave a specific anchor
        anchor.Erase((erasedAnchor, success) =>
        {
            var textComponents = erasedAnchor.GetComponentsInChildren<TextMeshProUGUI>();
            if (textComponents.Length > 1)
            {
                var savedStatusText = textComponents[1];
                savedStatusText.text = "Not Saved";	
            }
        });
    }

    private void ClearAllUuidsFromPlayerPrefs()
    {
        // Clear all UUIDs from player prefs
        if (PlayerPrefs.HasKey(NumUuidsPlayerPref))
        {
            int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
            for (int i = 0; i < playerNumUuids; i++)
            {
                PlayerPrefs.DeleteKey("uuid" + i);
            }
            PlayerPrefs.DeleteKey(NumUuidsPlayerPref);
            PlayerPrefs.Save();
        }
    }

    private void LoadSavedAnchors()
    {
        // Load all saved anchors
        LoadAnchorsByUuid();
    }

    private void LoadAnchorsByUuid()
    {
        // Load all saved anchors
        anchorLoader.LoadAnchorsByUuid();
    }

}

// public class SpatialAnchorManager : MonoBehaviour
// {
//     [NonSerialized]
//     public OVRSpatialAnchor anchorPrefab;
//     public GameObject[] anchorPrefabs; // Array of prefabs
//     private int currentPrefabIndex = 0; // Index of the current prefab to spawn  

//     public const string NumUuidsPlayerPref = "numUuids"; // Key for the number of UUIDs in PlayerPrefs     


//     private List<OVRSpatialAnchor> anchors = new List<OVRSpatialAnchor>();
//     private OVRSpatialAnchor lastCreatedAnchor;
//     private AnchorLoader anchorLoader;

//     // From the Awake method, get a reference to the AnchorLoader component (AnchorLoader.cs)
//     private void Awake() => anchorLoader = GetComponent<AnchorLoader>();

//     // Update is called once per frame
//     void Update()
//     {
//         // Create a spatial anchor when the primary index trigger is pressed
//         if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
//         {
//             CreateSpatialAnchor();
//         }

//         // Save the last created anchor when the One button is pressed
//         if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
//         {
//             SaveLastCreatedAnchor();
//         }

//         // Unsave the last created anchor when the Two button is pressed
//         if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
//         {
//             UnsaveLastCreatedAnchor();
//         }

//         // Unsave all anchors when the primary hand trigger is pressed
//         if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
//         {
//             UnsaveAllAnchors();
//         }

//         // Load all saved anchors when the primary R thumbstick button is pressed
//         if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
//         {
//             LoadSavedAnchors();
//         }

//         if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
//         {
//             CreateSpatialAnchor();
//         }

//         if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
//         {
//             currentPrefabIndex = (currentPrefabIndex + 1) % anchorPrefabs.Length;
//         }
//         else if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
//         {
//             currentPrefabIndex = (currentPrefabIndex - 1 + anchorPrefabs.Length) % anchorPrefabs.Length;
//         }

//     }

//     public void CreateSpatialAnchor()
//     {
//         anchorPrefab = anchorPrefabs[currentPrefabIndex].GetComponent<OVRSpatialAnchor>();
//         // Instantiate a new spatial anchor at the position and rotation of the right touch controller
//         GameObject workingAnchor = Instantiate(anchorPrefabs[currentPrefabIndex], OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
//             OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));

            
//         StartCoroutine(AnchorCreated(workingAnchor.GetComponent<OVRSpatialAnchor>())); // Call the coroutine to handle the anchor creation
//     }

//     private IEnumerator AnchorCreated(OVRSpatialAnchor workingAnchor)
//     {
//         // Wait until the anchor is created and localized
//         while (!workingAnchor.Created && !workingAnchor.Localized)
//         {
//             yield return new WaitForEndOfFrame();
//         }

//         // Get the UUID of the anchor and update the UI elements
//         Guid anchorGuid = workingAnchor.Uuid;
//         anchors.Add(workingAnchor);
//         Debug.Log("UUID: " + anchorGuid.ToString());
//         lastCreatedAnchor = workingAnchor;
//     }


//     // private IEnumerator AnchorCreated(GameObject workingAnchor)
//     // {
//     //     OVRSpatialAnchor spatialAnchor = workingAnchor.GetComponent<OVRSpatialAnchor>();
//     //     if (spatialAnchor == null)
//     //     {
//     //         Debug.LogError("OVRSpatialAnchor component not found on the working anchor.");
//     //         yield break;
//     //     }

//     //     // Wait until the anchor is created and localized
//     //     while (!spatialAnchor.Created && !spatialAnchor.Localized)
//     //     {
//     //         yield return new WaitForEndOfFrame();
//     //     }

//     //     // Get the UUID of the anchor and update the UI elements
//     //     Guid anchorGuid = spatialAnchor.Uuid;
//     //     anchors.Add(spatialAnchor);
//     //     lastCreatedAnchor = spatialAnchor;
//     // }


//     private void SaveLastCreatedAnchor()
//     {
//         // Save the last created anchor and update the saved status text
//         lastCreatedAnchor.Save((lastCreatedAnchor, success) =>
//         {
//             if (success)
//             {
//                 // savedStatusText.text = "Saved";
//                 Debug.Log("Saved");
//             } else
//             {
//                 Debug.Log("Not Saved");
//             }
//         });
//         SaveUuidToPlayerPrefs(lastCreatedAnchor.Uuid);
//     }

//     void SaveUuidToPlayerPrefs(Guid uuid)
//     {
//         if (!PlayerPrefs.HasKey(NumUuidsPlayerPref))
//         {
//             PlayerPrefs.SetInt(NumUuidsPlayerPref, 0);
//             Debug.Log("NumUuidsPlayerPref: " + PlayerPrefs.GetInt(NumUuidsPlayerPref));
//         }
//         int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
//         PlayerPrefs.SetString("uuid" + playerNumUuids, uuid.ToString());
//         PlayerPrefs.SetInt(NumUuidsPlayerPref, ++playerNumUuids);
//     }

//     private void UnsaveLastCreatedAnchor()
//     {
//         // Unsave the last created anchor and update the saved status text
//         lastCreatedAnchor.Erase((lastCreatedAnchor, success) =>
//         {
//             if (success)
//             {
//                 // savedStatusText.text = "Not Saved";
//                 Debug.Log("Not Saved");
//             }
//         });
//     }

//     private void UnsaveAllAnchors()
//     {
//         // Unsave all anchors and update the saved status text
//         foreach (var anchor in anchors)
//         {
//             UnsaveAnchor(anchor);
//         }
//     }

//     private void UnsaveAnchor(OVRSpatialAnchor anchor)
//     {
//         anchor.Erase((erasedAnchor, success) =>
//         {
//             var textComponents = erasedAnchor.GetComponentsInChildren<TextMeshProUGUI>();
//             if (textComponents.Length > 1)
//             {
//                 var savedStatusText = textComponents[1];
//                 savedStatusText.text = "Not Saved";	
//             }
//         });
//     }

//     private void ClearAllUuidsFromPlayerPrefs()
//     {
//         if (PlayerPrefs.HasKey(NumUuidsPlayerPref))
//         {
//             int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
//             for (int i = 0; i < playerNumUuids; i++)
//             {
//                 PlayerPrefs.DeleteKey("uuid" + i);
//             }
//             PlayerPrefs.DeleteKey(NumUuidsPlayerPref);
//             PlayerPrefs.Save();
//         }
//     }

//     private void LoadSavedAnchors()
//     {
//         // Load all saved anchors
//         anchorLoader.LoadAnchorsByUuid();
//     }

    
// }

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class SpatialAnchorManager : MonoBehaviour
// {
//     public OVRSpatialAnchor anchorPrefab;
//     public const string NumUuidsPlayerPref = "numUuids";        

//     public GameObject[] prefab;
//     private Canvas canvas;
//     private TextMeshProUGUI uuidText;
//     private TextMeshProUGUI savedStatusText;
//     private List<OVRSpatialAnchor> anchors = new List<OVRSpatialAnchor>();
//     private OVRSpatialAnchor lastCreatedAnchor;
//     private AnchorLoader anchorLoader;
//     private int currentAnchorIndex = 0;

//     private void Awake() => anchorLoader = GetComponent<AnchorLoader>();

//     // Update is called once per frame
//     void Update()
//     {
//         // Create a spatial anchor when the primary index trigger is pressed
//         if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
//         {
//             CreateSpatialAnchor();
//         }

//         // Save the last created anchor when the One button is pressed
//         if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
//         {
//             SaveLastCreatedAnchor();
//         }

//         // Unsave the last created anchor when the Two button is pressed
//         if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
//         {
//             UnsaveLastCreatedAnchor();
//         }

//         // Unsave all anchors when the primary hand trigger is pressed
//         if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
//         {
//             UnsaveAllAnchors();
//         }

//         // Load all saved anchors when the primary R thumbstick button is pressed
//         if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
//         {
//             LoadSavedAnchors();
//         }
//         // Check for input to cycle through anchors
//         if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
//         {
//             CycleSpatialAnchors();
//         }
//     }

//     public void CreateSpatialAnchor()
//     {
//                 // Instantiate a new spatial anchor at the position and rotation of the right touch controller
//         OVRSpatialAnchor workingAnchor = Instantiate(anchorPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
//             OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));

//         // Get references to the UI elements in the anchor's canvas
//         canvas = workingAnchor.gameObject.GetComponentInChildren<Canvas>();
//         uuidText = canvas.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
//         savedStatusText = canvas.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

//         StartCoroutine(AnchorCreated(workingAnchor));
//     }

//     private IEnumerator AnchorCreated(OVRSpatialAnchor workingAnchor)
//     {
//         // Coroutine logic for anchor creation
//                 // Wait until the anchor is created and localized
//         while (!workingAnchor.Created && !workingAnchor.Localized)
//         {
//             yield return new WaitForEndOfFrame();
//         }

//         // Get the UUID of the anchor and update the UI elements
//         Guid anchorGuid = workingAnchor.Uuid;
//         anchors.Add(workingAnchor);
//         lastCreatedAnchor = workingAnchor;

//         uuidText.text = "UUID: " + anchorGuid.ToString();
//         savedStatusText.text = "Not Saved";
//     }

//     private void SaveLastCreatedAnchor()
//     {
//         // Save the last created anchor
//         // Save the last created anchor and update the saved status text
//         lastCreatedAnchor.Save((lastCreatedAnchor, success) =>
//         {
//             if (success)
//             {
//                 savedStatusText.text = "Saved";
//             }
//         });
//         SaveUuidToPlayerPrefs(lastCreatedAnchor.Uuid);
//     }

//     void SaveUuidToPlayerPrefs(Guid uuid)
//     {
//         // Save the UUID to player prefs
//         if (!PlayerPrefs.HasKey(NumUuidsPlayerPref))
//         {
//             PlayerPrefs.SetInt(NumUuidsPlayerPref, 0);
//         }
//         int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
//         PlayerPrefs.SetString("uuid" + playerNumUuids, uuid.ToString());
//         PlayerPrefs.SetInt(NumUuidsPlayerPref, ++playerNumUuids);
//     }

//     private void UnsaveLastCreatedAnchor()
//     {
//         // Unsave the last created anchor and update the saved status text
//         lastCreatedAnchor.Erase((lastCreatedAnchor, success) =>
//         {
//             if (success)
//             {
//                 savedStatusText.text = "Not Saved";
//             }
//         });
//     }

//     private void UnsaveAllAnchors()
//     {
//         // Unsave all anchors and update the saved status text
//         foreach (var anchor in anchors)
//         {
//             UnsaveAnchor(anchor);
//         }
//     }

//     private void UnsaveAnchor(OVRSpatialAnchor anchor)
//     {
//         // Unsave a specific anchor
//         anchor.Erase((erasedAnchor, success) =>
//         {
//             var textComponents = erasedAnchor.GetComponentsInChildren<TextMeshProUGUI>();
//             if (textComponents.Length > 1)
//             {
//                 var savedStatusText = textComponents[1];
//                 savedStatusText.text = "Not Saved";	
//             }
//         });
//     }

//     private void ClearAllUuidsFromPlayerPrefs()
//     {
//         // Clear all UUIDs from player prefs
//         if (PlayerPrefs.HasKey(NumUuidsPlayerPref))
//         {
//             int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
//             for (int i = 0; i < playerNumUuids; i++)
//             {
//                 PlayerPrefs.DeleteKey("uuid" + i);
//             }
//             PlayerPrefs.DeleteKey(NumUuidsPlayerPref);
//             PlayerPrefs.Save();
//         }
//     }

//     private void LoadSavedAnchors()
//     {
//         // Load all saved anchors
//         anchorLoader.LoadAnchorsByUuid();
//     }

//     private void CycleSpatialAnchors()
//     {
//         // Cycle through the array of spatial anchors
//         if (anchors.Count > 0)
//         {
//             currentAnchorIndex = (currentAnchorIndex + 1) % anchors.Count;
//             OVRSpatialAnchor currentAnchor = anchors[currentAnchorIndex];
//             // Do something with the current anchor
//         }
//     }
// }


