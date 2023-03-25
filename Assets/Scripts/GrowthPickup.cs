using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthPickup : MonoBehaviour, ICollectable
{
    [SerializeField] private float _growthAmount = 0.05f;
    private bool _canCollect;
    
    public bool CanCollect 
    {
        get => _canCollect;
        set => _canCollect = value;
    }
    
    public void Collect(GameObject player)
    {
        if (!CanCollect)
            return;
        
        player.GetComponent<PlayerGrowth>()?.Grow(_growthAmount);
        Destroy(gameObject);
    }
}
