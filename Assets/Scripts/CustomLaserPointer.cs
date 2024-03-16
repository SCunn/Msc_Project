using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Editor.BuildingBlocks;
using UnityEngine;

public class CustomLaserPointer : MonoBehaviour
{
    [SerializeField]
    private Transform controllerAnchor; // The anchor point for the laser pointer.

    [SerializeField]
    private OVRInput.RawButton objectsDeleteAction; // The button to toggle object visibility.

    [Header("Line Render Settings")]
    [SerializeField]
    private float lineWidth = 0.01f; // The width of the laser pointer line.

    [SerializeField]
    private float lineMaxLength = 50f; // The maximum length of the laser pointer line.

    private RaycastHit hit; // Stores information about the raycast hit.

    private LineRenderer lineRenderer; // The line renderer component to draw the laser pointer line.



    private void Awake()
    {
        // Create a new line renderer component and set its properties.
        lineRenderer = gameObject.GetComponent<LineRenderer>(); // Get the line renderer component.
        lineRenderer.widthMultiplier = lineWidth; // Set the width of the line renderer.
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 anchorPosition = controllerAnchor.position; // Get the position of the controller anchor.
        Quaternion anchorRotation = controllerAnchor.rotation; // Get the rotation of the controller anchor.

        // if the raycast hits an object, set the end point of the line renderer to the hit point.
        if (Physics.Raycast(new Ray(anchorPosition, anchorRotation * Vector3.forward), out hit, lineMaxLength))
        {
            GameObject objectHit = GameObject.FindWithTag("SpatialAnchor"); // Get the game object that was hit by the raycast.
            
            // Destroy Spatial Anchor
            if (OVRInput.GetDown(objectsDeleteAction, OVRInput.Controller.RTouch))
            {
                if (objectHit != null)
                {
                    Destroy(objectHit);
                }
            }

        }
        else 
        {
            hit.point = anchorPosition + (anchorRotation * Vector3.forward * lineMaxLength); // Set the end point of the line renderer to the maximum length.
            lineRenderer.SetPosition(0, anchorPosition);    // Set the starting position of the line renderer to the anchor position.
            lineRenderer.SetPosition(1, anchorPosition + anchorRotation * Vector3.forward * lineMaxLength); // Set the ending position of the line renderer to the maximum length in front of the anchor.
        }
    }
}
