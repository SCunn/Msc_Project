using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTracking : MonoBehaviour
{
    public GameObject camera;

    public GameObject leftController, rightController;

    public GameObject prefab;



    // Update is called once per frame
    void Update()
    {
        // Controller Tracking
        leftController.transform.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        leftController.transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

        rightController.transform.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        rightController.transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);

        // when handtracking is active generate a prefab at the last known localposition and localrotation of the controllers, when the controllers are active, destroy the prefab
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) || OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            GameObject prefab = Instantiate(camera, leftController.transform.localPosition, leftController.transform.localRotation);
            prefab.transform.parent = leftController.transform;
        }
        else
        {
            Destroy(GameObject.Find("OVRPlayerController"));
        }

    }
}
