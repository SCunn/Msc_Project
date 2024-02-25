using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{

    public OVRInput.Button button1;
    // public OVRInput.Button button2;
    public OVRInput.Controller controller;
    
    [Header("References")]
    [SerializeField] private Transform[] items;

    [Header("Keys")]
    [SerializeField] private KeyCode[] keys;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedItem;
    private float timeSinceLastSwitch;
    // Start is called before the first frame update
    private void Start()
    {
       SetItems();
       Select(selectedItem);

       timeSinceLastSwitch = 0f;
    }


    private void SetItems()
    {
        items = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++){
            items[i] = transform.GetChild(i);
        }
        if (keys == null) keys = new KeyCode[items.Length];
    }
    // Update is called once per frame
    private void Update()
    {
        int previousSelectedItem = selectedItem;

        for (int i = 0; i < keys.Length; i++){
        // if (OVRInput.GetDown(button1))
            if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime)
                selectedItem = i;
        }
        if (previousSelectedItem != selectedItem) Select(selectedItem);

        timeSinceLastSwitch += Time.deltaTime;
       
    }
    private void Select(int itemIndex)
    {
        for (int i = 0; i < items.Length; i++){
            items[i].gameObject.SetActive(i == itemIndex);
        }
        timeSinceLastSwitch = 0f;
    }

}
