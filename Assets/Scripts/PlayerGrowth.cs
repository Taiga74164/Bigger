using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGrowth : MonoBehaviour
{
    Vector3 targetScale;

    [SerializeField]
    private float growthRate;


    // Start is called before the first frame update
    void Start()
    { 
        targetScale = transform.localScale;
    }

    public void Grow(float growthAmount)
    {
        targetScale += new Vector3(growthAmount, growthAmount, growthAmount);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(targetScale);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, growthRate * Time.deltaTime);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        ICollectable collectable = collision.gameObject.GetComponent<ICollectable>();
        if (collectable != null)
        {
            collectable.Collect(gameObject);
        }
    }

    //write async function to handle growth
}
