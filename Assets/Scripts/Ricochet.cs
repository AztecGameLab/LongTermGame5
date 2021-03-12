using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ricochet", menuName = "Ricochet")]
public class Ricochet : ProjectileWeapon
{
    public GameObject ricochet;
    GameObject newRicochet;
    public Transform firePosition;
    private float speed = 10.0f; 
    private Rigidbody2D projectileHitbox;
    private int maxReflectionCount = 5; //i dont know how many reflections you want
    private int damage;
    Vector2 newDirection;


    public override void Fire()
    {
        newRicochet = Instantiate(ricochet, firePosition.position, firePosition.rotation);
        projectileHitbox = GetComponent<Rigidbody2D>();
        projectileHitbox.velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Entity>())
        {
            GameObject.Destroy(this.gameObject, 0);
        }
        else
        {
            if (maxReflectionCount != 0)
            {
                Vector2 normalForce = other.contacts[0].normal;
                newDirection = Vector2.Reflect(projectileHitbox.velocity, normalForce).normalized;

                projectileHitbox.velocity = newDirection * speed;
                maxReflectionCount = maxReflectionCount - 1;
            }
            else
            {
                GameObject.Destroy(this.gameObject, 0);
            }
        }
    }
    
}
