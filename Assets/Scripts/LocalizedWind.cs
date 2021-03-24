using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizedWind : MonoBehaviour
{
    List<Rigidbody2D> RigidbodiesinWindZoneList = new List<Rigidbody2D>();
    public Vector2 windDirection = Vector2.right;
    public float windStrength = 5;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Trigger");
        Rigidbody2D objectRigid = col.gameObject.GetComponent<Rigidbody2D>();
        if (objectRigid != null)
            RigidbodiesinWindZoneList.Add(objectRigid);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Rigidbody2D objectRigid = col.gameObject.GetComponent<Rigidbody2D>();
        if (objectRigid != null)
            RigidbodiesinWindZoneList.Remove(objectRigid);
    }

    private void FixedUpdate()
    {
            foreach (Rigidbody2D rigid in RigidbodiesinWindZoneList)
            {
            windDirection.Normalize();
                rigid.AddForce(windDirection * windStrength);
            }   
    }
}
