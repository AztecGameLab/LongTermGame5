using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointGrapple : MonoBehaviour
{
    
    public Vector2 grappleFire;
    public DistanceJoint2D distance;
    public LayerMask whatIsGrapplable;
    public Camera cam;


    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
            StopGrapple();
        }

    private void StartGrapple()
    {

        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos,10000, whatIsGrapplable);

        if (hit.collider != null)
        {
            Debug.Log("ray casted");
            Debug.DrawRay(transform.position, mousePos, Color.green);
            Debug.Log("rigid body hit");
            Vector3 coll = hit.collider.ClosestPoint(hit.point);
            distance.connectedAnchor = coll;
            
        }


    }

    private void StopGrapple()
    {
        distance.breakForce = 0;
    }
}
