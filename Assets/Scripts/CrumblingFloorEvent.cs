using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingFloorEvent : MonoBehaviour
{
    public float crumbleDelay;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("there was a collision!");
        if (collision.CompareTag("Player"))
        {

            Debug.Log("destroying object after one second");
            Destroy(gameObject.transform.parent.gameObject, crumbleDelay);
        }
    }
}
