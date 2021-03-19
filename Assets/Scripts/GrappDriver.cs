using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappDriver : MonoBehaviour
{
    public GrappScript Grapp;
    Vector2 direction;
 

    
    // Update is called once per frame
    void Update()
    {

        Grapp.Charge(direction);
    }
}
