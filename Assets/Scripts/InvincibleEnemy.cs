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
    public bool moveRight;
    private float startPos;
    private float endPos;
    Vector3 playerPos;
    bool Co_active;
    bool enabled;
    bool aggro;

    // Start is called before the first frame update
    public void Awake()
    {
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        enemyTransform = GetComponent<Transform>();
        Co_active = false;
        moveRight = false;
        startPos = transform.position.x - Radius;
        endPos = startPos + Radius;
        aggro = false;
    }

    void OnBecameVisible()
    {
        enabled = true;
    }
    void OnBecameInvisible()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            playerPos = PlatformerController.instance.transform.position;
            if(Vector3.Distance(playerPos, enemyTransform.position) < 3)
            {
                aggro = true;
            }
            else
            {
                aggro = false;
            }
            if (enemyRigidBody2D.position.x >= endPos* (-1* Convert.ToInt32(aggro)) + playerPos.x* Convert.ToInt32(aggro))
            {
                moveRight = false;
            }
            if (enemyRigidBody2D.position.x <= startPos * (-1 * Convert.ToInt32(aggro)) + playerPos.x * Convert.ToInt32(aggro))
                moveRight = true;
            



            if (!Co_active)
            {
                StartCoroutine("Jump");
            }
            /*else
            {
                if (moveRight)
                {
                    enemyRigidBody2D.AddForce(Vector2.right * EnemySpeed * Time.deltaTime);

                }
                else
                {
                    enemyRigidBody2D.AddForce(-Vector2.right  * EnemySpeed * Time.deltaTime);
                }
            } */





            if (playerPos.x < enemyTransform.position.x)
            {
                print("ENEMY TO RIGHT");
            }
            if (playerPos.x > enemyTransform.position.x)
            {
                print("ENEMY TO LEFT");
            }
            if ((playerPos.y - enemyTransform.position.y) < -1)
            {
                print("ENEMY ABOVE");
            }
            if ((playerPos.y - enemyTransform.position.y) > -1)
            {
                print("ENEMY BELOW");
            }
            
        }
        
            

    }
    IEnumerator Jump()
    {
        Co_active = true;
        yield return new WaitForSeconds(3);
        enemyRigidBody2D.velocity = Vector2.zero;
        enemyRigidBody2D.AddForce(Vector2.up *250* EnemySpeed * Time.deltaTime);
        if (moveRight)
        {
            enemyRigidBody2D.AddForce(Vector2.right * 100 * EnemySpeed * Time.deltaTime);

        }
        else
        {
            enemyRigidBody2D.AddForce(-Vector2.right * 100 * EnemySpeed * Time.deltaTime);
        }

        Co_active = false;

    }
    public override void TakeDamage(float baseDamage)
    {
        if(playerPos.x > enemyTransform.position.x)
        {
            base.TakeDamage(baseDamage);
        }
            
    }

}
