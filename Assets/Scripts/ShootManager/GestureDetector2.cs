// This code was sourced from Marco Paoloni GestureDetector.cs, https://gitlab.com/tnd-public/tndscripts/-/blob/master/Unity/Scripts/Gesture%20Detectors/GestureDetector.cs?ref_type=heads
using System.Collections;  // System.Collections is used to call the IEnumerator interface
using System.Collections.Generic;  // System.Collections.Generic is used to call the List class
using UnityEngine;         // Unity Engine is used to call the Unity API, this is used to call the Unity functions and classes
using UnityEngine.Events;  // Unity Event System is used to call functions from the inspector without the need of a reference to a script component


[System.Serializable] // This allows the struct to be shown in the inspector

// Gesture struct, contains the name of the gesture, a list of vector3's, and a unity event
public struct Gesture
{
    public string name;     // String to store the name of the gesture
    public List<Vector3> fingerDatas;   // List of Vector3's to store the finger data
    public UnityEvent onRecognized;     // UnityEvent to store the onRecognized event
}

// GestureDetector class
public class GestureDetector2 : MonoBehaviour
{
    // Variables
    // float value to determine how accurate the recognition should be
    [Header("Threshold value")]
    public float threshold = 0.1f;

    // OVRSkeleton is a class that contains the bones of the hand
    [Header("Hand Skeleton")]
    public OVRSkeleton skeleton;

    // List of Gestures declared as gestures
    [Header("Gestures")]
    public List<Gesture> gestures;

    // List of OVRBones declared as fingerBones set to null
    private List<OVRBone> fingerBones = null;

    //Bool to determine if the program is in debug mode
    [Header("Debug Mode")]
    public bool debugMode = true;

    // Aditional bollean values to determine if the code is working correctly
    private bool hasStarted = false;   // Bool to determine if the gesture has started
    private bool hasRecognized = false;  // Bool to determine if the gesture has been recognized
    private bool hasFinished = false;  // Bool to determine if the gesture has finished

    // Add the option to create an event when a gesture is not recognized
    [Header("Not Recognized Event")]
    public UnityEvent notRecognized;

    // Start is called before the first frame update
    void Start()
    {
        //// Start the DelayRoutine to initialize the gesture after 2.5 seconds
        //StartCoroutine(DelayRoutine(2.5f, Initialize));

        // Start Coroutine to operate the initialization of the gesture functionality outside of the main thread
        StartCoroutine(UntilInitialized());  
    }

    // Create IEnumerator to delay the initialization of the gesture

    //public IEnumerator DelayRoutine(float delay, Action actionToDo)
    //{
    //    // Wait for the delay, in this case 2.5 seconds, before invoking the actionToDo
    //    yield return new WaitForSeconds(delay);
    //    // Invoke the actionToDo
    //    actionToDo.Invoke();
    //}

    public IEnumerator UntilInitialized()
    {
    // While the OVRBones are not initialized, return a null value, otherwise call Intialize function to begin gesture recognitions
        while (!skeleton.IsInitialized)
        {
            yield return null;
        }
        Initialize();
    }

    // Create Initialize function to initialize the gesture
    public void Initialize()
    {
        // Call the SetSkeleton function
        SetSkeleton();
        // Set the hasStarted bool to true
        hasStarted = true;
    }

    //Create SetSkeleton function this will set the fingerBones list to the bones of the hand
    public void SetSkeleton()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
    }

    // Update is called once per frame
    void Update()
    {
        //If debugMode is true and the space key is pressed start the save function, this is used to save the gesture when space bar is pressed during game mode in the viewport
        if (debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            //fingerBones = new List<ovrbone>(skeleton.bones);  // fingerbones is set to a new list of ovrbones
            //SetSkeleton(); // Call the SetSkeleton function to set the fingerBones list to the bones of the hand
            Save(); // Call the Save function to save the gesture
        }

        // If the inintialization was successful
        if (hasStarted.Equals(true))
        {
            // start to Recognize every gesture the user makes
            Gesture currentGesture = Recognize();

            // Use hasRecognized boolean to check if the gesture has been already saved on the gesture list
            hasRecognized = !currentGesture.Equals(new Gesture());

            // If the gesture is recognized
            if (hasRecognized)
            {
                hasFinished = true; // set hasFinished to true to avoid the event from looping
                // invoke an event if on is present
                currentGesture.onRecognized?.Invoke();
            }
            // If the gesture created is no longer recognized
            else
            {
                // If the gesture hasFinished is set to true
                if (hasFinished)
                {
                    Debug.Log("Not Recognized"); // Print Not Recognized to the console
                    // Set hasFinished to false
                    hasFinished = false;
                    // Invoke a notRecognized event, this can be used to call an event when the previos gesture event has finished
                    notRecognized?.Invoke();
                }
            }
        } 
    }

    // Create Save function to save the gesture
    void Save()
    {
        // Create a new Gesture struct
        Gesture g = new Gesture();

        // Set the name of the gesture to the name of the game object
        g.name = "New Gesture";

        // Create a new list of vector3's called data
        List<Vector3> data = new List<Vector3>();

        // For each bone in fingerBones
        foreach (var bone in fingerBones)
        {
            // Add the position of the bone to the data list
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }
        // Set the fingerDatas of the gesture to the data list
        g.fingerDatas = data;
        // Add the gesture to the gestures list
        gestures.Add(g);
    }

    // Recognize function, this function will be called when the gesture is recognized
    Gesture Recognize()
    {
        // Create a new gesture called currentGesture
        Gesture currentGesture = new Gesture();
        // Create a float called currentMin
        float currentMin = Mathf.Infinity;
        
        // create For each loop inside the gestures list
        foreach (var gesture in gestures)
        {
            float sumDistance = 0; // Create a float called sumdistance and set it to 0
            bool isDiscarded = false; // Create a bool called isDiscarded and set it to false

            // Create a For loop to loop through the fingerBones list initialized in Setskeleton function
            for (int i = 0; i < fingerBones.Count; i++)
            {
                // Set currentData to the position of the bone
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                
                // Set distance to the distance between the currentData and the gesture data
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]); 

                // If the distance is greater than the threshold then discard the gesture
                if (distance > threshold)
                {
                    isDiscarded = true; // Set isDiscarded to true
                    break;              // Break out of the loop
                }

                // If the distance is correct, Add the distance to the sumDistance
                sumDistance += distance;
            }

            //If the gesture is not discarded and the sumDistance is less than the currentMin
            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;       // Set currentMin to the sumDistance
                currentGesture = gesture;       // Set currentGesture to the gesture
            }
        }
        return currentGesture;                  // Return the currentGesture
    } 
}
