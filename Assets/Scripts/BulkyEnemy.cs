using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulkyEnemy : Entity
{
    float healthTrigger;
    Rigidbody2D enemyRigidBody2D;
    Transform enemyTransform;
    public float EnemySpeed = 2;
    Vector3 playerPos;
    int attackRange;
    bool lowHealth;

    public void Awake()
    {
        attackRange = 3;
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        enemyTransform = GetComponent<Transform>();

    }

    // Start is called before the first frame update
    void Start()
    {
        lowHealth = false;
        healthTrigger = health / 2;
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


    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if (!lowHealth && health < healthTrigger)  //Player is unable to attck currently, so I can't quite test this fully
            {
                lowHealth = true;
                EnemySpeed *= 2;
                attackRange *= 2;
            }
            playerPos = PlatformerController.instance.transform.position;
            if (enemyRigidBody2D.position.x >= playerPos.x)
            {
                enemyRigidBody2D.AddForce(Vector2.left * 5 * EnemySpeed * Time.deltaTime);
            }
            else
            {
                enemyRigidBody2D.AddForce(Vector2.right * 5 * EnemySpeed * Time.deltaTime);
            }
            if (Vector3.Distance(playerPos, enemyTransform.position) < attackRange)
            {
                print("Attack"); //Not quite sure how we will be implementing attack?
            }
        }
        
        
    }
}
