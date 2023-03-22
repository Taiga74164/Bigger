using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthPickup : MonoBehaviour, ICollectable
{
    [SerializeField]
    private float growthAmount = 0.05f;

    public void Collect(GameObject _player)
    {
        if(_player.GetComponent<PlayerGrowth>())
        {
            _player.GetComponent<PlayerGrowth>().Grow(growthAmount);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
