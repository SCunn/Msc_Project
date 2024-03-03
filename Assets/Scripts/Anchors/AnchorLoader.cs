using System;
using TMPro;
using UnityEngine;

// This class is responsible for loading anchors in the Unity scene.
public class AnchorLoader : MonoBehaviour
{
    private GameObject anchorPrefab; // Prefab for the spatial anchor
    // private SpatialAnchorManager spatialAnchorManager; // Reference to the spatial anchor manager

    Action<OVRSpatialAnchor.UnboundAnchor, bool> _onLoadAnchor; // Delegate for handling anchor loading

    private void Awake()
    {
        anchorPrefab = GetComponent<SpatialAnchorManager>().anchorPrefabs[0]; // Get the spatial anchor manager component
        // anchorPrefab = spatialAnchorManager.anchorPrefab; // Get the anchor prefab from the manager
        _onLoadAnchor = OnLocalized; // Assign the OnLocalized method to the delegate
    }

    /// Loads anchors based on UUIDs stored in PlayerPrefs.
    public void LoadAnchorsByUuid()
    {
        if (!PlayerPrefs.HasKey(SpatialAnchorManager.NumUuidsPlayerPref))
        {
            PlayerPrefs.SetInt(SpatialAnchorManager.NumUuidsPlayerPref, 0); // Reset the UUID count in PlayerPrefs
        }

        var playerUuidCount = PlayerPrefs.GetInt(SpatialAnchorManager.NumUuidsPlayerPref); // Get the UUID count from PlayerPrefs

        if (playerUuidCount == 0)
            return;

        var uuids = new Guid[playerUuidCount]; // Array to store the UUIDs
        for (int i = 0; i < playerUuidCount; ++i)
        {
            var uuidKey = "uuid" + i;
            var currentUuid = PlayerPrefs.GetString(uuidKey); // Get the UUID from PlayerPrefs

            uuids[i] = new Guid(currentUuid); // Convert the UUID string to Guid and store in the array
        }

        Load(new OVRSpatialAnchor.LoadOptions
        {
            Timeout = 0,
            StorageLocation = OVRSpace.StorageLocation.Local,
            Uuids = uuids // Set the UUIDs to load
        });
    }
    // This method loads the anchors based on the UUIDs stored in PlayerPrefs
    private void Load(OVRSpatialAnchor.LoadOptions options)
    {
        OVRSpatialAnchor.LoadUnboundAnchors(options, anchors =>
        {
            if (anchors == null)
            {
                return;
            }

            foreach (var anchor in anchors)
            {
                if (anchor.Localized)
                {
                    _onLoadAnchor(anchor, true); // Call the delegate for localized anchors
                }
                else if (!anchor.Localizing)
                {
                    anchor.Localize(_onLoadAnchor);
                }
            }
        });
    }

    // This method is called when an anchor is localized meaning when an anchor is loaded in the scene
    private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success)
    {
        if (!success) return;

        var pose = unboundAnchor.Pose;
        var spatialAnchorObject = Instantiate(anchorPrefab, pose.position, pose.rotation); // Instantiate the anchor prefab
        var spatialAnchor = spatialAnchorObject.GetComponent<OVRSpatialAnchor>();
        unboundAnchor.BindTo(spatialAnchor); // Bind the unbound anchor to the instantiated anchor

        if (spatialAnchor.TryGetComponent<OVRSpatialAnchor>(out var anchor))
        {
            var UUidText = spatialAnchor.GetComponentInChildren<TextMeshProUGUI>();
            var savedStatusText = spatialAnchor.GetComponentsInChildren<TextMeshProUGUI>()[1];

            UUidText.text = "UUID: " + spatialAnchor.Uuid.ToString(); // Set the UUID text
            savedStatusText.text = "Loaded from Device"; // Set the status text
        }
    }

    
}
// /// This class is responsible for loading anchors in the Unity scene.
// public class AnchorLoader : MonoBehaviour
// {
//     private OVRSpatialAnchor anchorPrefab; // Prefab for the spatial anchor
//     private SpatialAnchorManager spatialAnchorManager; // Reference to the spatial anchor manager

//     Action<OVRSpatialAnchor.UnboundAnchor, bool> _onLoadAnchor; // Delegate for handling anchor loading

//     private void Awake()
//     {
//         spatialAnchorManager = GetComponent<SpatialAnchorManager>(); // Get the spatial anchor manager component
//         anchorPrefab = spatialAnchorManager.anchorPrefab; // Get the anchor prefab from the manager
//         _onLoadAnchor = OnLocalized; // Assign the OnLocalized method to the delegate
//     }

//     /// Loads anchors based on UUIDs stored in PlayerPrefs.
//     public void LoadAnchorsByUuid()
//     {
//         if (!PlayerPrefs.HasKey(SpatialAnchorManager.NumUuidsPlayerPref))
//         {
//             PlayerPrefs.SetInt(SpatialAnchorManager.NumUuidsPlayerPref, 0); // Reset the UUID count in PlayerPrefs
//         }

//         var playerUuidCount = PlayerPrefs.GetInt(SpatialAnchorManager.NumUuidsPlayerPref); // Get the UUID count from PlayerPrefs

//         if (playerUuidCount == 0)
//             return;

//         var uuids = new Guid[playerUuidCount]; // Array to store the UUIDs
//         for (int i = 0; i < playerUuidCount; ++i)
//         {
//             var uuidKey = "uuid" + i;
//             var currentUuid = PlayerPrefs.GetString(uuidKey); // Get the UUID from PlayerPrefs

//             uuids[i] = new Guid(currentUuid); // Convert the UUID string to Guid and store in the array
//         }

//         Load(new OVRSpatialAnchor.LoadOptions
//         {
//             Timeout = 0,
//             StorageLocation = OVRSpace.StorageLocation.Local,
//             Uuids = uuids // Set the UUIDs to load
//         });
//     }
//     // This method loads the anchors based on the UUIDs stored in PlayerPrefs
//     private void Load(OVRSpatialAnchor.LoadOptions options)
//     {
//         OVRSpatialAnchor.LoadUnboundAnchors(options, anchors =>
//         {
//             if (anchors == null)
//             {
//                 return;
//             }

//             foreach (var anchor in anchors)
//             {
//                 if (anchor.Localized)
//                 {
//                     _onLoadAnchor(anchor, true); // Call the delegate for localized anchors
//                 }
//                 else if (!anchor.Localizing)
//                 {
//                     anchor.Localize(_onLoadAnchor); // Localize the anchor and call the delegate when done
//                 }
//             }
//         });
//     }
//     // This method is called when an anchor is localized meaning when an anchor is loaded in the scene
//     private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success)
//     {
//         if (!success) return;

//         var pose = unboundAnchor.Pose;
//         var spatialAnchor = Instantiate(anchorPrefab, pose.position, pose.rotation); // Instantiate the anchor prefab
//         unboundAnchor.BindTo(spatialAnchor); // Bind the unbound anchor to the instantiated anchor

//         if (spatialAnchor.TryGetComponent<OVRSpatialAnchor>(out var anchor))
//         {
//             var UUidText = spatialAnchor.GetComponentInChildren<TextMeshProUGUI>();
//             var savedStatusText = spatialAnchor.GetComponentsInChildren<TextMeshProUGUI>()[1];

//             UUidText.text = "UUID: " + spatialAnchor.Uuid.ToString(); // Set the UUID text
//             savedStatusText.text = "Loaded from Device"; // Set the status text
//         }
//     }
// }

