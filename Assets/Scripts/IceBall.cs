using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag != "Player") { 
        collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        Destroy(gameObject);
        StartCoroutine(Unfreeze(collision));
       // collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;  
        }
    }
     IEnumerator Unfreeze(Collision2D collision)
    {
     // collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        Debug.Log("Made it");
        yield return new WaitForSeconds(5.0f);
     collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        Debug.Log("unfreeze");

    }
}