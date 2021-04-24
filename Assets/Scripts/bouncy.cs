using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class bouncy : MonoBehaviour
{
    public float springForce = 500;
    // private Collision2D collision;
    // private bool bouncing = false;
    [EventRef] public string bounceSound;
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.rigidbody)
        {
            GetComponentInChildren<Animator>().SetTrigger("Bounce");
            
            RuntimeManager.PlayOneShot(bounceSound, transform.position);

            if (coll.gameObject.GetComponent<PlatformerController>())
            {
                coll.rigidbody.velocity = new Vector2(0, springForce);
                //coll.rigidbody.AddForce(new Vector2(0, springForce * 2));
            }
            else
            {
                coll.rigidbody.velocity = new Vector2(0, springForce);
                //coll.rigidbody.AddForce(new Vector2(0, springForce));
            }
        }
        
        // if (!bouncing)
        // {
        //     bouncing = true;
        //     collision = coll;
        // }
    }

    // void FixedUpdate()
    // {
    //     if (bouncing)
    //     {
    //         var rb = collision.gameObject.GetComponent<Rigidbody2D>();
    //         rb.velocity = new Vector3(0, 0, 0);
    //         rb.AddForce(new Vector2(0f, springForce));
    //         bouncing = false;
    //     }
    // }
}



