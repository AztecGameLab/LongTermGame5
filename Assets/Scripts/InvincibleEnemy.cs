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
    public void Start()
    {
        StartCoroutine("Jump");
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
        /*if(Vector3.Distance(playerPos, enemyTransform.position) < 3)
        {
            aggro = true;
        }
        else
        {
            aggro = false;
        }*/
        
            



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





        
            
    }
    IEnumerator Jump()
    {
        while (true)
        {

            playerPos = PlatformerController.instance.transform.position;
            enemyRigidBody2D.velocity = Vector2.zero;
            //if (enemyRigidBody2D.position.x >= endPos* (-1* Convert.ToInt32(aggro)) + playerPos.x* Convert.ToInt32(aggro))
            if (enemyRigidBody2D.position.x >= playerPos.x)
            {
                moveRight = false;
            }
            //if (enemyRigidBody2D.position.x <= startPos * (-1 * Convert.ToInt32(aggro)) + playerPos.x * Convert.ToInt32(aggro))
            if (enemyRigidBody2D.position.x <= playerPos.x)
            {
                moveRight = true;
            }
                
            enemyRigidBody2D.AddForce(Vector2.up * 350 * EnemySpeed * Time.deltaTime);
            if (moveRight)
            {
                enemyRigidBody2D.AddForce(Vector2.right * 100 * EnemySpeed * Time.deltaTime);

            }
            else
            {
                enemyRigidBody2D.AddForce(-Vector2.right * 100 * EnemySpeed * Time.deltaTime);
            }
            yield return new WaitForSeconds(3);
        }
        yield return null;

        

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
