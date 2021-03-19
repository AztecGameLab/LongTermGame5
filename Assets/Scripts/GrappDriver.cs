using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappDriver : MonoBehaviour
{
    [SerializeField]  GrappScript Grapp;
    Vector2 direction;
    public Camera cam;




    // Update is called once per frame
    private void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)transform.position;

        if (Input.GetMouseButtonDown(0))
            Grapp.Fire(direction);

        else if (Input.GetMouseButtonUp(0))
            Grapp.EndGrapple();
    }
}

