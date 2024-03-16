using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace MetaAdvancedFeatures.SceneUnderstanding
{
    public class OVRSceneManagerAddons : MonoBehaviour
    {
        protected OVRSceneManager SceneManager { get; private set; }           // Reference to the OVRSceneManager component

        private void Awake()
        {
            SceneManager = GetComponent<OVRSceneManager>();
        }

        void Start()
        {
            SceneManager.SceneModelLoadedSuccessfully += OnSceneModelLoadedSuccessfully;
        }

        private void OnSceneModelLoadedSuccessfully()
        {
            StartCoroutine(AddCollidersAndFixClassifications());
        }

        // This method adds colliders to all objects with MeshRenderer component and fixes the orientation of desks.
        private IEnumerator AddCollidersAndFixClassifications()
        {
            // [Note] jackyangzzh: to avoid racing condition, wait for end of frame
            //                     for all prefabs to be populated properly before continuing
            yield return new WaitForEndOfFrame();

            // Find all objects with MeshRenderer component
            MeshRenderer[] allObjects = FindObjectsOfType<MeshRenderer>();

            // Add BoxCollider component to objects without a Collider component
            foreach (var obj in allObjects)
            {
                if (obj.GetComponent<Collider>() == null)
                {
                    obj.AddComponent<BoxCollider>();
                }
                // remove colliders from objects with the "Ignore" tag
                if (obj.gameObject.CompareTag("Ignore"))
                {
                    Destroy(obj.GetComponent<Collider>());
                }
            }

            // Fix the orientation of desks by flipping their scale on the z-axis
            OVRSemanticClassification[] allClassifications = FindObjectsOfType<OVRSemanticClassification>()
                .Where(c => c.Contains(OVRSceneManager.Classification.Table))
                .ToArray();

            foreach(var classification in allClassifications)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * -1);
            }

            // Function to add stencil buffer to windows and doors
            // AddStencilBufferToWindowsAndDoors();


        }

        // This method adds a stencil buffer to all objects with the "Window" or "Door" tag
        // private void AddStencilBufferToWindowsAndDoors()
        // {
        //     // Find all objects with the "Window" or "Door" tag
        //     GameObject[] allWindowsAndDoors = GameObject.FindGameObjectsWithTag("Window")
        //         .Concat(GameObject.FindGameObjectsWithTag("Door"))
        //         .ToArray();

        //     // Add a Stencil Buffer component to all objects with the "Window" or "Door" tag
        //     foreach (var obj in allWindowsAndDoors)
        //     {
        //         if (obj.GetComponent<StencilBuffer>() == null)
        //         {
        //             obj.AddComponent<StencilBuffer>();
        //         }
        //     }
        // }

    }
}
