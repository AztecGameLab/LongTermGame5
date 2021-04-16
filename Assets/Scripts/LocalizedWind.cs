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
            RigidbodiesinWindZoneList.Add(objectRigid);
            if (col.gameObject.tag == "Player")
            {
                WindBecomesAggressive();
            }
        }
            
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        WindBecomesPassive();
        Rigidbody2D objectRigid = col.gameObject.GetComponent<Rigidbody2D>();
        if (objectRigid != null)
        {
            RigidbodiesinWindZoneList.Remove(objectRigid);
            if(col.gameObject.tag == "Player")
            {
                WindBecomesPassive();
            }
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
        // This will cause the wind SFX to sound calm
        windEmitter.SetParameter("Wind State", 0);
    }

    private void WindBecomesAggressive()
    {
        // This will cause the wind SFX to sound more aggressive
        windEmitter.SetParameter("Wind State", 1);
    }
}
