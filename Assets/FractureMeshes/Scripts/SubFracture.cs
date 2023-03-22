using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshCollider))]
public class SubFracture : MonoBehaviour
{
    FractureNetworkNode node;

    //get component references
    Rigidbody _rb;

    MeshCollider _collider;

    //fracture network
    public FractureNetwork _network;
    public FractureNetworkNode _node;

    void Start()
    {
        _network = GetComponentInParent<FractureNetwork>(); //get fracture network

        _rb = GetComponent<Rigidbody>();        //get rigid body or create one if none exists
        if (!_rb)
        {
            _rb = gameObject.AddComponent<Rigidbody>();
        }
        
        _rb.isKinematic = true; //turn off physics until broken

        _collider = GetComponent<MeshCollider>();//add collider if none exists
        if (!_collider)
        {
            _collider = gameObject.AddComponent<MeshCollider>();
            //collider should be convex for complex subfractures
            //like those generated by voronoi fracture
            //if you have a fractured mesh it is likely the pieces will be convex
            _collider.convex = true;
        }
    }

    private void Update()
    {
        Break();
    }

    public void Break()
    {
        if (_node.isBroken)
        {
            _rb.isKinematic = false;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red; //set mesh to red if broken
            StartCoroutine(EnablePickup());
        }  
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_node.isBroken)
        {
            if (collision == null) return;
            //if (_node.isFoundation) return;

            if (collision.impulse.magnitude > 2.5f) //if collision is of sufficient force
            {
                _node.isBroken = true;
                _network.StartCollapse(); //tell the fracture network to start collapsing

                return;
            }
        }
    }

    IEnumerator EnablePickup()
    {
        yield return new WaitForSeconds(3); //delay to let the rubble fall

        //once collection is enabled, the player will scoop up the rubble around them

        if(GetComponent<GrowthPickup>()) GetComponent<GrowthPickup>().enabled = true;
        if(GetComponent<ICollectable>() != null) GetComponent<ICollectable>().CanCollect = true;
    }

}
