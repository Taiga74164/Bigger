using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This behaviour uses a trigger volume to pull collectables in 
 * and collect them when they hit the collider. 
 * 
 * This script assumes the collectable is using the ICollectable interface
 */
public class SmoothCollection : MonoBehaviour
{
    [SerializeField]
    private float PullStrengh = 5.0f;
    [SerializeField]
    private float PullRadius = 10f;


    private void OnTriggerStay(Collider other)
    {
        ICollectable collectable = other.gameObject.GetComponent<ICollectable>();
        
        //validity checks
        if (collectable == null) return;
        if (!collectable.CanCollect) return;

        //pull other object towards the collecting player
        other.transform.position = Vector3.Lerp(other.transform.position, transform.position, PullStrengh * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ICollectable collectable = collision.gameObject.GetComponent<ICollectable>(); //collectable interface

        //validity chekcs
        if (collectable == null) return;
        if (!collectable.CanCollect) return;

        //on collision collect the collectable
        collectable.Collect(gameObject);
    }
}
