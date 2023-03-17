using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Forces the current object to look at the specified object.
/// </summary>
public class LookAt : MonoBehaviour
{
    public Transform lookAt;
    public bool invert;
    public bool mainCamera;

    private void Start()
    {
        // Look at the main camera if no object is specified.
        if (mainCamera)
            lookAt = GameObject.FindWithTag("MainCamera").transform;
    }

    private void Update()
    {
        try
        {
            // Look at the specified transform.
            transform.LookAt((invert ? 2 : 1) * transform.position - lookAt.position);
        }
        catch (UnassignedReferenceException) { }
    }
}