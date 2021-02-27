using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointGrapple : MonoBehaviour
{
    
    public Vector2 grappleFire;
    public SpringJoint2D spring;
    public LayerMask whatIsGrapplable;


    private void Update()
    {
        //spring.attachedRigidbody.position.x -= 1;
      //  spring.attachedRigidbody.position.y -= 1;
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
            StopGrapple();
        }

    private void StartGrapple()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Input.mousePosition, whatIsGrapplable);
        
        if (hit.collider != null)
        {
            Debug.Log("ray casted");
            Debug.DrawRay(transform.position, Input.mousePosition, Color.green);
            if (hit.rigidbody != null)
            {
                Debug.Log("rigid body hit");
                Vector3 coll = hit.collider.ClosestPoint(hit.point);
                spring.connectedAnchor = coll;
            }
        }


    }

    private void StopGrapple()
    {
        throw new NotImplementedException();
    }
}
