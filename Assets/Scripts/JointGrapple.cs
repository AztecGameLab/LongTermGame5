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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos, grappleRange, whatIsGrapplable);                               //Raycast to mouse position using the camera
        
        if (hit.collider != null)
        {
            joint = player.gameObject.AddComponent<SpringJoint2D>();

            Vector2 grapplePoint = hit.collider.ClosestPoint(hit.point);

            joint.enableCollision = true;
            joint.autoConfigureConnectedAnchor = false;                         //grapple settings
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector2.Distance(player.position, grapplePoint);

            joint.frequency = grappleSpeed;
            joint.distance = grappleTightness;
            joint.dampingRatio = dampingRatio;
            
        }


    }

    private void StopGrapple()
    {
        Destroy(joint);
    }
}
