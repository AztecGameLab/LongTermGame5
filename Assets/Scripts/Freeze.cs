using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        Debug.Log("Made it");
        collider.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        var sr = GetComponentInChildren<SpriteRenderer>();
        var go = Instantiate(Resources.Load("Ice") as GameObject, sr.bounds.center, quaternion.identity);
        go.transform.localScale = Vector3.one * sr.bounds.size.y;
        
        yield return new WaitForSeconds(5.0f);
        
        Destroy(go);
        collider.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        Debug.Log("unfreeze");
        Destroy(this);
    }


}
