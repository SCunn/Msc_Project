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
            }

            // Fix the orientation of desks by flipping their scale on the z-axis
            OVRSemanticClassification[] allClassifications = FindObjectsOfType<OVRSemanticClassification>()
                .Where(c => c.Contains(OVRSceneManager.Classification.Table))
                .ToArray();

            foreach(var classification in allClassifications)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * -1);
            }
        }
    }
}
