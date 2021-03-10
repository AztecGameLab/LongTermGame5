using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPound : MonoBehaviour
{

    public float dropForce = 5f;
    public float stopTime = 0.5f;
    public float gravityScale = 1f;
    private PlatformerController player;
    private Rigidbody2D body;
    private bool doGroundPound = false;
    //get animator later

    private void Awake()
    {
        player = GetComponent<PlatformerController>();
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //works
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!player.isGrounded)
            {
                doGroundPound = true;
            }
        }


    }

    private void FixedUpdate()
    {
        if (doGroundPound)
        {
            GroundPoundAttack();
            Debug.Log("this is working!");
        }

        doGroundPound = false;
    }

    //if the player collides with something, then the ground pound stops
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.contacts[0].normal.y >= 0.5)
        {
            CompleteGroundPound();
        }
    }

    public void GroundPoundAttack()
    {
        player.enabled = false;
        StopAndSpin();
        StartCoroutine("DropAndSmash");
    }

    //stop in the air and play animation
    public void StopAndSpin()
    {
        ClearForces();
        body.gravityScale = 0;
        //animation line here while spinning/whatever
    }

    //wait for some seconds in the air, then push downward for the smash
    private IEnumerator DropAndSmash()
    {
        yield return new WaitForSeconds(stopTime);
        body.AddForce(Vector2.down * dropForce, ForceMode2D.Impulse);
        //animation line here while dropping down
    }

    //End of ground pound
    public void CompleteGroundPound()
    {
        body.gravityScale = gravityScale;
        player.enabled = true;
        //animation line here to stop the drop animation
    }

    public void ClearForces()
    {
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
    }

}
