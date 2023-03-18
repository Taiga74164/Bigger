using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Forces the current object to look at the specified object.
/// </summary>
public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform _lookAt;
    public bool Invert;
    public bool MainCamera;

    private void Start()
    {
        // Look at the main camera if no object is specified.
        if (MainCamera)
            _lookAt = GameObject.FindWithTag("MainCamera").transform;
    }

    private void Update()
    {
        try
        {
            // Look at the specified transform.
            transform.LookAt((Invert ? 2 : 1) * transform.position - _lookAt.position);
        }
        catch (UnassignedReferenceException) { }
    }
}