using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        Debug.Log("Made it");
        collider.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(5.0f);
        collider.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        Debug.Log("unfreeze");
        Destroy(this);
    }


}
