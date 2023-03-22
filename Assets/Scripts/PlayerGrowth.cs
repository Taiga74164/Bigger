using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGrowth : MonoBehaviour
{
    float currentScale;
    float targetScale;

    [SerializeField]
    private float growthRate;


    // Start is called before the first frame update
    void Start()
    {
        currentScale = transform.localScale.x; //assume scale is uniform   
        targetScale = currentScale;
    }

    public void Grow(float growthAmount)
    {
        targetScale += growthAmount;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(targetScale);
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(targetScale, targetScale, targetScale), growthRate * Time.deltaTime);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        ICollectable collectable = collision.gameObject.GetComponent<ICollectable>();
        if (collectable != null)
        {
            collectable.Collect(gameObject);
        }
    }
}
