using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCollider : MonoBehaviour
{
    public GameObject fireball;
    public GameObject player;
    
    
    void Start()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.GetComponent<Entity>() != null)
        {
            col.gameObject.GetComponent<Entity>().TakeDamage(10);
        }
        Destroy(fireball);
    }
}
