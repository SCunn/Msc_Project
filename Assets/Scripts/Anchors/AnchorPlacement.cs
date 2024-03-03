using UnityEngine;

public class AnchorPlacement : MonoBehaviour
{
    public GameObject[] anchorPrefabs; // Array of prefabs
    private int currentPrefabIndex = 0; // Index of the current prefab to spawn

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            CreateSpatialAnchor();
        }

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            currentPrefabIndex = (currentPrefabIndex + 1) % anchorPrefabs.Length;
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            currentPrefabIndex = (currentPrefabIndex - 1 + anchorPrefabs.Length) % anchorPrefabs.Length;
        }
        
    }

    public void CreateSpatialAnchor()
    {
        // Instantiate the selected prefab at the controller position
        GameObject prefab = Instantiate(anchorPrefabs[currentPrefabIndex], OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
            OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));

        prefab.AddComponent<OVRSpatialAnchor>();

        // Cycle to the next prefab in the array
        // currentPrefabIndex = (currentPrefabIndex + 1) % anchorPrefabs.Length;
    }
}
        




// public class AnchorPlacement : MonoBehaviour
// {
//     public GameObject anchorPrefab;


//     // Update is called once per frame
//     void Update()
//     {
//         if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
//         {
//             CreateSpatialAnchor();
//         }
//     }

//     public void CreateSpatialAnchor()
//     {
//         GameObject prefab = Instantiate(anchorPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
//          OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
//         prefab.AddComponent<OVRSpatialAnchor>();
//     }
// }