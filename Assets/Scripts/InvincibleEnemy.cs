using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using System;

public class InvincibleEnemy : Entity
{
    Rigidbody2D enemyRigidBody2D;
    Transform enemyTransform;
    [EventRef] public string JumpSound, LandSound;
    public float EnemySpeed = 70;
    public float Strength = 5;
    public int Radius = 5;
    public bool moveRight, LEFT, RIGHT, UP, DOWN;
    Vector2 playerPos, enemyPosition;
    public Animator animator;
    private SpriteRenderer _spriteRenderer;
    public float agroRange = 30;

    // Start is called before the first frame update
    public void Awake()
    {
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        enemyTransform = GetComponent<Transform>();
        moveRight = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        enabled = false;
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
        float velocity = Mathf.Sqrt(Mathf.Abs(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a)));
        return velocity * direction.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        enemyRigidBody2D.velocity = Vector2.zero;
        RuntimeManager.PlayOneShot(LandSound);
        // animator.SetFloat("VelocityX", enemyRigidBody2D.velocity.x);
        // animator.SetFloat("VelocityY", enemyRigidBody2D.velocity.y);
        if (collision.rigidbody == PlatformerController.instance.GetComponent<Rigidbody2D>())
        {
            PlatformerController.instance.TakeDamage(Strength);
            //print("Success!!!");
        }
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

                float dist = Vector2.Distance(playerPos, enemyPosition);
                if (dist <= agroRange)
                {
                    //print(calculateHop(enemyPosition, playerPos));
                    enemyRigidBody2D.velocity = calculateHop(enemyPosition, playerPos, dist > 10 ? 45 : 75);
                    _spriteRenderer.flipX = enemyRigidBody2D.velocity.x < 0;
                    // animator.SetFloat("VelocityX", enemyRigidBody2D.velocity.x);
                    // animator.SetFloat("VelocityY", enemyRigidBody2D.velocity.y);
                    animator.SetTrigger("Jump");
                    RuntimeManager.PlayOneShot(JumpSound);
                }
            }

            yield return new WaitForSeconds(3);
        }
    }

    public override void TakeDamage(float baseDamage)
    {
        base.TakeDamage(baseDamage);

        
        // if (RIGHT && playerPos.x < enemyTransform.position.x)
        // {
        //     animator.SetTrigger("Damaged");
        //     base.TakeDamage(baseDamage);
        // }
        //
        // if (LEFT && playerPos.x > enemyTransform.position.x)
        // {
        //     animator.SetTrigger("Damaged");
        //     base.TakeDamage(baseDamage);
        // }
        //
        // if (UP && (playerPos.y - enemyTransform.position.y) < -1)
        // {
        //     animator.SetTrigger("Damaged");
        //     base.TakeDamage(baseDamage);
        // }
        //
        // if (DOWN && (playerPos.y - enemyTransform.position.y) > 1)
        // {
        //     animator.SetTrigger("Damaged");
        //     base.TakeDamage(baseDamage);
        // }
    }


    public override void OnDeath()
    {
        animator.SetBool("Dead", true);
        GameObject.Destroy(this.gameObject, 3);
    }
}