using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This behaviour allows the player to gradually grow and shrink to a target size.
/// </summary>
public class PlayerGrowth : MonoBehaviour
{
    [SerializeField] private float _growthRate = 1f;
    
    private PlayerController _playerController; 
    private Vector3 _targetScale;
    private float _targetMoveSpeed;
    private Coroutine _growthCoroutine;
    
    private void Start()
    { 
        _playerController = GetComponent<PlayerController>();
        _targetScale = transform.localScale;
        _targetMoveSpeed = _playerController.MoveSpeed;
        _growthCoroutine = StartCoroutine(Grow());
    }
    
    private void Update()
    {
        // Debug.Log(targetScale);
        // transform.localScale = Vector3.Lerp(transform.localScale, targetScale, growthRate * Time.deltaTime);
    }
    
    private void OnDestroy()
    {
        if (_growthCoroutine != null)
            StopCoroutine(_growthCoroutine);
    }
    
    public void Grow(float amount)
    {
        // Set the target scale and move speed.
        _targetScale += new Vector3(amount, amount, amount);
        _targetMoveSpeed += amount;
        
        // Update player attributes.
        _playerController.UpdateGrowth(amount);
    }
    
    private IEnumerator Grow()
    {
        while (true)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, _growthRate * Time.deltaTime);
            if (TryGetComponent(out PlayerController playerController))
                playerController.MoveSpeed = Mathf.Lerp(playerController.MoveSpeed, _targetMoveSpeed, _growthRate * Time.deltaTime);
            
            yield return new WaitForSeconds(0.05f);
        }
    }
}
