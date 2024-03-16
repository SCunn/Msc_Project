
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

    // // Delete selected anchor
    // public void DeleteSpatialAnchor()
    // {
    //     // Get the anchor to delete
    //     GameObject anchor = GameObject.FindWithTag("SpatialAnchor");

    //     // If an anchor was found, destroy it
    //     if (anchor != null)
    //     {
    //         Destroy(anchor);
    //     }
    // }

    // // Select created spatial anchor
    // public void SelectSpatialAnchor()
    // {
    //     // Get the anchor to select
    //     GameObject anchor = GameObject.FindWithTag("SpatialAnchor");

    //     // If an anchor was found, select it
    //     if (anchor != null)
    //     {
    //         anchor.GetComponent<OVRSpatialAnchor>().SetSelected(true);
    //     }
    // }
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