using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCollider : MonoBehaviour
{
    public GameObject fireball;
    public FireBallStats stats;
    
    void Start()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.GetComponent<Entity>() != null)
        {
            col.gameObject.GetComponent<Entity>().TakeDamage(this.stats.damage);
        }
        Destroy(fireball);
    }
}
