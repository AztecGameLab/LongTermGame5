using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using Unity.Mathematics;
using UnityEngine;
using FMODUnity;

public class FireBallCollider : MonoBehaviour
{
    public GameObject fireball;
    public FireBallStats stats;
    public GameObject explosion;
    public StudioEventEmitter fireballSound;

    void Start()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        fireballSound.SetParameter("Fireball", 2);
        Destroy(Instantiate(explosion, transform.position, quaternion.identity),0.2f);
        if(col.gameObject.GetComponent<Entity>() != null)
        {
            col.gameObject.GetComponent<Entity>().TakeDamage(this.stats.damage);
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(fireball.transform.position, 3);
        for(int i=0; i<colliders.Length; ++i)
        {
            
            if(colliders[i].gameObject.GetComponent<Rigidbody2D>() != null)
            {
                Debug.Log(colliders[i].gameObject.name);
                colliders[i].attachedRigidbody.velocity = (colliders[i].gameObject.transform.position - transform.position).normalized * stats.FireBallSize * 5;
            }
        }
        Destroy(fireball);
    }
}
