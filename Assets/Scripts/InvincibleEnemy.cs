using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InvincibleEnemy : Entity
{
    Rigidbody2D enemyRigidBody2D;
    Transform enemyTransform;
    public float EnemySpeed = 70;
    public int Radius = 5;
    public bool moveRight, LEFT, RIGHT, UP, DOWN;
    Vector2 playerPos, enemyPosition;
    

    // Start is called before the first frame update
    public void Awake()
    {
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        enemyTransform = GetComponent<Transform>();
        moveRight = false;
        
    }
    public void Start()
    {
        StartCoroutine("Jump");
        enabled = false;
    }
    void OnBecameVisible()
    {
        enabled = true;
    }
    void OnBecameInvisible()
    {
        enabled = false;
    }
    Vector3 calculateHop(Vector2 source, Vector2 target, float angle = 45)
    {
        Vector2 direction = target - source;
        float h = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float a = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        // calculate velocity
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * direction.normalized;
    }


    IEnumerator Jump()
    {
        while (true)
        {
            if (enabled)
            {
                enemyRigidBody2D.velocity = Vector2.zero;
                playerPos = PlatformerController.instance.transform.position;
                enemyPosition = enemyTransform.position;
                print(calculateHop(enemyPosition, playerPos));
                enemyRigidBody2D.velocity = calculateHop(enemyPosition, playerPos);
            }
            
            yield return new WaitForSeconds(3);
        }


        

    }
    public override void TakeDamage(float baseDamage)
    {
       
        if (RIGHT && playerPos.x < enemyTransform.position.x)
        {
            base.TakeDamage(baseDamage);
        }
        if (LEFT && playerPos.x > enemyTransform.position.x)
        {
            base.TakeDamage(baseDamage);
        }
        if (UP && (playerPos.y - enemyTransform.position.y) < -4)
        {
            base.TakeDamage(baseDamage);
        }
        if (DOWN && (playerPos.y - enemyTransform.position.y) > 4)
        {
            base.TakeDamage(baseDamage);
        }

    }

}
