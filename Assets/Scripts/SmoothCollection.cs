using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This behaviour uses a trigger volume to pull collectables in and collect them when they hit the collider.
/// This script assumes the collectable is using the ICollectable interface
/// </summary>
public class SmoothCollection : MonoBehaviour
{
    [SerializeField] private float _pullStrength = 5.0f;
    [SerializeField] private float _pullRadius = 10f;
    
    [SerializeField] private bool _useLerp;
    
    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out ICollectable collectable)) return;
        if (!collectable.CanCollect) return;
        
        // Pull other object towards the collecting player.
        if (!_useLerp)
        {
            var direction = transform.position - other.transform.position;
            if (direction.magnitude > _pullRadius) return;
            
            other.transform.position += direction.normalized * _pullStrength * Time.deltaTime;
        }
        else
        {
            other.transform.position = Vector3.Lerp(other.transform.position, transform.position, _pullStrength * Time.deltaTime);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out ICollectable collectable)) return;
        if (!collectable.CanCollect) return;
        
        // On collision collect the collectable.
        collectable.Collect(gameObject);
    }
}
