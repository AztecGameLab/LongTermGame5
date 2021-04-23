using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class LocalizedWind : MonoBehaviour
{
    List<Rigidbody2D> RigidbodiesinWindZoneList = new List<Rigidbody2D>();
    public Vector2 windDirection = Vector2.right;
    public float windStrength = 5;

    public StudioEventEmitter windEmitter;

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        Rigidbody2D objectRigid = col.gameObject.GetComponent<Rigidbody2D>();
        if (objectRigid != null)
        {
          
            if (col.gameObject.CompareTag("Player"))
            {
                WindBecomesAggressive();
            }
            RigidbodiesinWindZoneList.Add(objectRigid);
        }
            
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        WindBecomesPassive();
        Rigidbody2D objectRigid = col.gameObject.GetComponent<Rigidbody2D>();
        if (objectRigid != null)
        {
            
            if(col.gameObject.CompareTag("Player"))
            {
                WindBecomesPassive();
            }
            RigidbodiesinWindZoneList.Remove(objectRigid);
        }
    }

    private void FixedUpdate()
    {
            foreach (Rigidbody2D rigid in RigidbodiesinWindZoneList)
            {
            windDirection.Normalize();
            rigid.AddForce(windDirection * windStrength);
            }   
    }


    private void WindBecomesPassive()
    {
        
        windEmitter.SetParameter("Wind State", 0);
    }

    private void WindBecomesAggressive()
    {
        
        windEmitter.SetParameter("Wind State", 1);
    }
}
