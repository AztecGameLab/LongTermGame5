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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(fireball.transform.position, fireball.GetComponent<CircleCollider2D>().radius*5);
        for(int i=0; i<colliders.Length; ++i)
        {
            
            if(colliders[i].gameObject.GetComponent<Rigidbody2D>() != null)
            {
                Debug.Log(colliders[i].gameObject.name);
                colliders[i].attachedRigidbody.AddForce(colliders[i].gameObject.transform.up * 10, ForceMode2D.Impulse);
            }
        }
        Destroy(fireball);
    }
}
