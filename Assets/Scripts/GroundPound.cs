using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundPound : Ability
{

    public float dropForce =10f;
    public float stopTime = 0.5f;
    public float gravityScale = 1f;
    private Rigidbody2D body;
    private bool doingGroundPound = false;
    //get animator later

    protected override string InputName => "GroundPound";

    

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }


    // private void FixedUpdate()
    // {
    //     doingGroundPound = false;
    // }

    //if the player collides with something, then the ground pound stops
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(doingGroundPound)
            print(other.contacts[0].normal);
        if (other.contacts[0].normal.y >= 0.5 && doingGroundPound)
        {
            CompleteGroundPound();
        }
    }

    protected override void Started(InputAction.CallbackContext context)
    {
        doingGroundPound = true;
        Player.lockControls = true;
        StopAndSpin();
        StartCoroutine(DropAndSmash());
    }

    //stop in the air and play animation
    public void StopAndSpin()
    {
        ClearForces();
        body.gravityScale = 0;
        GetComponent<Animator>().Play("groundPound");
        //animation line here while spinning/whatever
    }

    //wait for some seconds in the air, then push downward for the smash
    private IEnumerator DropAndSmash()
    {
        yield return new WaitForSeconds(stopTime);
        while (doingGroundPound)
        {
            body.AddForce(Vector2.down * dropForce, ForceMode2D.Force);
            yield return null;
        }
        //body.AddForce(Vector2.down * dropForce, ForceMode2D.Impulse);
        //animation line here while dropping down
    }

    //End of ground pound
    public void CompleteGroundPound()
    {
        GetComponent<Animator>().SetTrigger("groundPoundDone");
        body.gravityScale = gravityScale;
        Player.lockControls = false;
        doingGroundPound = false;
        //animation line here to stop the drop animation
    }

    public void ClearForces()
    {
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
    }

}
