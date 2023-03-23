using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * This behaviour allows the player to gradually grow and shrink to a target size. 
 */

public class PlayerGrowth : MonoBehaviour
{
    PlayerController tpc; 

    Vector3 targetScale;
    float targetMoveSpeed;

    [SerializeField]
    private float growthRate;


    // Start is called before the first frame update
    void Start()
    { 
        tpc = GetComponent<PlayerController>();
        targetScale = transform.localScale; //initialize scale so player doesn't shrink
        targetMoveSpeed = tpc.MoveSpeed;
        StartCoroutine(Grow());
    }

    public void Grow(float growthAmount)
    {
        targetScale += new Vector3(growthAmount, growthAmount, growthAmount);
        targetMoveSpeed += growthAmount;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(targetScale);
        //transform.localScale = Vector3.Lerp(transform.localScale, targetScale, growthRate * Time.deltaTime);
        
    }

    /*
     * Move this function onto a collection monobehaviour that "scoops" up collectables with a trigger volume
     */

    private void OnCollisionEnter(Collision collision)
    {
        ICollectable collectable = collision.gameObject.GetComponent<ICollectable>();
        if (collectable != null)
        {
            collectable.Collect(gameObject);
        }
    }

    IEnumerator Grow()
    {
        while(true)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, growthRate * Time.deltaTime);
            ThirdPersonController tpc = GetComponent<ThirdPersonController>();
            if (tpc)
            {
                tpc.MoveSpeed = Mathf.Lerp(tpc.MoveSpeed, targetMoveSpeed, growthRate * Time.deltaTime);
            }
            yield return new WaitForSeconds(.05f);
        }
    }

    //write async function to handle growth to improve performance
}
