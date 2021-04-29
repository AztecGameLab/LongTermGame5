using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrappScript", menuName = "LTG5/Weapons/GrappleHook", order = 0)]
public class GrappScript : ProjectileWeapon
{
    SpringJoint2D joint;
    public LineRenderer lr;
    private float _rope;

    override
    public void Fire(Vector2 direction)
    {
        lr = PlatformerController.instance.gameObject.AddComponent<LineRenderer>();
        StartGrapple(direction);
    }

    public override void Cancel()
    {
        EndGrapple();
    }

    public void EndGrapple()            
    {

        //lr.positionCount = 0;
        Destroy(lr);
        Destroy(joint);
    }

    public void StartGrapple(Vector2 direction)
    {
        Vector2 playerPos = PlatformerController.instance.transform.position;
        //Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;      for testing while i didnt have PlatformerController on
        RaycastHit2D hit = Physics2D.Raycast(playerPos, direction, 100);                           

        Debug.DrawRay(playerPos, direction, Color.red, 10);
        if (hit.collider != null)
        {
  

            joint = PlatformerController.instance.gameObject.AddComponent<SpringJoint2D>();

            joint.enableCollision = true;
            joint.autoConfigureConnectedAnchor = false;                         //grapple settings
            joint.connectedAnchor = hit.collider.ClosestPoint(hit.point);
            _rope = (Vector2.Distance(PlatformerController.instance.gameObject.transform.position, joint.connectedAnchor));
            joint.frequency = 1;
            joint.distance = _rope / 1.3f;    //divided by float so it can start swinging even if it on the ground by pulling the player up slightly
            joint.dampingRatio = 1;

            lr.positionCount = 2;

            PlatformerController.instance.StartCoroutine(DrawRope());
        }

    }

    IEnumerator DrawRope()
    {
        lr.material.color = Color.red;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        while (joint)
        {
            lr.SetPosition(0, PlatformerController.instance.transform.position);
            lr.SetPosition(1, joint.connectedAnchor);
            yield return null;
        }
    }

}

    

