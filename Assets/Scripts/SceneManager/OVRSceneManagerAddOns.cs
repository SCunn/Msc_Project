using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using Unity.AI.Navigation;
using UnityEngine;


namespace MetaAdvancedFeatures.SceneUnderstanding
{
    public class OVRSceneManagerAddons : MonoBehaviour
    {
        [SerializeField]
        private NavMeshSurface navSurface;
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
                // // remove colliders from objects with the "Ignore" tag
                // if (obj.gameObject.CompareTag("Ignore"))
                // {
                //     Destroy(obj.GetComponent<Collider>());
                //     // // Add a cube collider to the object, positioned beind and bleow the floor level of the object
                //     // BoxCollider boxCollider = obj.gameObject.AddComponent<BoxCollider>();	
                //     // boxCollider.size = new Vector3(1.0f, 0.3f, 0.2f);
                //     // boxCollider.center = new Vector3(0.0f, -1.95f, -0.1f);
                //     // // Add a Transform component to the object, positioned in front of the object
                //     // Transform transform = obj.gameObject.AddComponent<Transform>();
                //     // transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                //     // transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                //     // transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //     // // Add a Transform component to the object, positioned behind the object
                //     // Transform transform2 = obj.gameObject.AddComponent<Transform>();
                //     // transform2.position = new Vector3(0.0f, 0.0f, 0.0f);
                //     // transform2.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                //     // transform2.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    


                //     // OffMeshLink offMeshLink = obj.AddComponent<OffMeshLink>();
                //     // offMeshLink.startTransform = startPoint.transform;
                //     // offMeshLink.endTransform = endPoint.transform;
                //     // offMeshLink.costOverride = 2.0f;
                //     // offMeshLink.activated = true;
                // }

                // if (obj.gameObject.CompareTag("Floor"))
                // {
                //     navSurface.BuildNavMesh();
                // }
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
