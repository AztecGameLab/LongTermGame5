using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointGrapple : MonoBehaviour
{
    public LayerMask whatIsGrapplable;
    public Camera cam;
    SpringJoint2D joint;
    public Transform player;
    public float grappleRange = 100, grappleTightness = 2, dampingRatio = 1, grappleSpeed = 0;
    public LineRenderer lr;
    Vector2 grapplePoint;

    private void Start() { 
        lr = GetComponent<LineRenderer>();
    }

    private void Update() { 
        DrawRope();

        if (Input.GetMouseButtonDown(0))
            StartGrapple();

        else if (Input.GetMouseButtonUp(0))
            StopGrapple();
        }


    private void StartGrapple() { 

        
        Vector2 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos, grappleRange, whatIsGrapplable);                               //Raycast to mouse position using the camera

        Debug.DrawRay(transform.position, mousePos, Color.red, 10);
        if (hit.collider != null)
        {
            //Debug.Log("collider hit");
            
            joint = player.gameObject.AddComponent<SpringJoint2D>();

            grapplePoint = hit.collider.ClosestPoint(hit.point);

            joint.enableCollision = true;
            joint.autoConfigureConnectedAnchor = false;                         //grapple settings
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector2.Distance(player.position, grapplePoint);

            joint.frequency = grappleSpeed;
            joint.distance = grappleTightness;
            joint.dampingRatio = dampingRatio;

            lr.positionCount = 2;
        }


    }

    private void StopGrapple() { 
        lr.positionCount = 0;
        Destroy(joint);
    }

    private void DrawRope() { 
        //dont draw rope if there is no joint
        if (!joint)             
            return;
        
        lr.SetPosition(0, player.position);
        lr.SetPosition(1, grapplePoint);
    }
}
