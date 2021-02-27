using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringGrapple : MonoBehaviour
{
    private SpringJoint2D player;
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpringJoint2D>();
            player.connectedBody = GetComponent<Rigidbody2D>();
        }
    }
}
