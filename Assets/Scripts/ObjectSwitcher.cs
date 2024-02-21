
using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    
    public int currentObject = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(currentObject <= 0)
                currentObject = 0;
            else
                currentObject++;    
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(currentObject <=0 )
                currentObject = transform.childCount - 1;
            else
                currentObject--;
        }
    }

    void SelectObject(){
        int i = 0;
        foreach (Transform obj in transform)
        {
           if (i == currentObject)
                obj.gameObject.SetActive(true);
              else
                obj.gameObject.SetActive(false);
              i++; 
        }
    }
}
