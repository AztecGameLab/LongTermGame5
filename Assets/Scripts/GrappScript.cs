using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappScript : ProjectileWeapon
{
    SpringJoint2D joint;
    Vector2 grapplePoint;
    public LineRenderer lr;
    public float grappleRange = 100, grappleTightness = 2, dampingRatio = 1, grappleSpeed = 0;

    override
    public void Charge(Vector2 direction)
    {
        Debug.Log("entered Charge");
        PlatformerController.instance.StartCoroutine(StartGrapple(direction));
    }

    public IEnumerator StartGrapple(Vector2 direction)
    {
        Vector2 origin = GameObject.FindGameObjectWithTag("Player").transform.position;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction);
        //Do something
        while (Input.GetMouseButtonDown(0))
        {
            if (hit.collider != null)
            {
                //Debug.Log("collider hit");

                joint = GameObject.FindGameObjectWithTag("Player").gameObject.AddComponent<SpringJoint2D>();

                grapplePoint = hit.collider.ClosestPoint(hit.point);

                joint.enableCollision = true;
                joint.autoConfigureConnectedAnchor = false;                         //grapple settings
                joint.connectedAnchor = grapplePoint;

                // float distanceFromPoint = Vector2.Distance(player.position, grapplePoint);

                joint.frequency = grappleSpeed;
                joint.distance = grappleTightness;
                joint.dampingRatio = dampingRatio;

                lr.positionCount = 2;
                //Do something repetitively 
                yield return null; //Wait for next update loop
            }//I can still put code here
        }
    }
}
