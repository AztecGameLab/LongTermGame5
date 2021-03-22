using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizedWind : MonoBehaviour
{
    List<Rigidbody2D> RigidbodiesinWindZoneList = new List<Rigidbody2D>();
    public Vector3 windDirection = Vector3.right;
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
        if (RigidbodiesinWindZoneList.Count > 0)
        {
            foreach (Rigidbody2D rigid in RigidbodiesinWindZoneList)
            {
                rigid.AddForce(windDirection * windStrength);
            }
        }
    }
}
