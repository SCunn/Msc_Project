using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class DynamicNavMesh : MonoBehaviour
{
    [SerializeField] 
    private NavMeshSurface surface;

    // Update is called once per frame
    // void Update()
    // {
    //     surface.BuildNavMesh();
    // }

    // create coroutine to update navmesh every 5 seconds
    void Start()
    {
        StartCoroutine(UpdateNavMesh());
    }

    IEnumerator UpdateNavMesh()
    {
        while(true)
        {
            surface.BuildNavMesh();
            yield return new WaitForSeconds(5);
        }
    }
}
