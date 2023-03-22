using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthPickup : MonoBehaviour, ICollectable
{
    [SerializeField]
    private float growthAmount = 0.05f;

    public bool CanCollect { get { return _canCollect; } set { _canCollect = value; } }

    private bool _canCollect;

    private void Start()
    {
        _canCollect = false;
    }

    public void Collect(GameObject _player)
    {
        if (!_canCollect) return;

        if(_player.GetComponent<PlayerGrowth>())
        {
            _player.GetComponent<PlayerGrowth>().Grow(growthAmount);
            Destroy(gameObject);
        }
    }
}
