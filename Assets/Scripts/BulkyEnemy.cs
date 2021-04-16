using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulkyEnemy : Entity
{
    float healthTrigger;
    Rigidbody2D enemyRigidBody2D;
    Transform enemyTransform;
    //public BoxCollider2D AttackHitBox;
    public float EnemySpeed;
    Vector3 playerPos;
    float attackRange = 2;
    public bool lowHealth;
    private bool isAttacking, PlayerIsLeft;
    public Animator animator;
    private SpriteRenderer _spriteRenderer;
    public float damage = 1;

    public void Awake()
    {
        //enabled = false;
        attackRange = 3;
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        enemyTransform = GetComponent<Transform>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        //AttackHitBox.offset = new Vector2(0f, 0f); // Left 
        //AttackHitBox.size = new Vector2(1f, 2f); //Small
    }

    // Start is called before the first frame update
    void Start()
    {
        lowHealth = false;
        healthTrigger = health / 2;
        isAttacking = false;
        PlayerIsLeft = false;
        animator.SetBool("Walking", true);
        GetComponentInChildren<BulkyEnemyEvent>().BulkyEnemy = this;

    }
    // void OnBecameVisible()
    // {
    //     enabled = true;
    // }
    // void OnBecameInvisible()
    // {
    //     enabled = false;
    // }
    public void AttackEnd()
    {
        var hits = Physics2D.CircleCastAll(transform.position, 1, PlayerIsLeft ? Vector2.left : Vector2.right, attackRange);
        foreach (var hit in hits)
        {
            var pc = hit.transform.GetComponent<PlatformerController>();
            if(pc)
                pc.TakeDamage(damage);
        }
        isAttacking = false;
        //AttackHitBox.offset = new Vector2(0f, 0f); // Left 
        //AttackHitBox.size = new Vector2(1f, 2f); //Small
    }
    bool WhichSideIsPlayer()
    {
        return enemyRigidBody2D.position.x >= playerPos.x;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("HIT");
    }
    // void BulkyAttack(bool direction)
    // {
    //     
    //     if (!lowHealth)
    //     {
    //         if (direction)
    //         {
    //             AttackHitBox.offset = new Vector2(-1.5f, -.5f); // Left 
    //             AttackHitBox.size = new Vector2(2f, 1f); //Small
    //         }
    //         else
    //         {
    //             AttackHitBox.offset = new Vector2(1.5f, -.5f); // Right
    //             AttackHitBox.size = new Vector2(2f, 1f); //Small
    //         }
    //     }
    //     else
    //     {
    //         if (direction)
    //         {
    //             AttackHitBox.offset = new Vector2(-2f, 0f); // Left 
    //             AttackHitBox.size = new Vector2(3f, 2f); //Large
    //         }
    //         else
    //         {
    //             AttackHitBox.offset = new Vector2(2f, 0f); // Right 
    //             AttackHitBox.size = new Vector2(3f, 2f); //Large
    //         }
    //     }
    //
    // }
    

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if (!lowHealth && health < healthTrigger)  //Player is unable to attck currently, so I can't quite test this fully
            {
                lowHealth = true;
                EnemySpeed *= 2;
            }
            playerPos = PlatformerController.instance.transform.position;
            PlayerIsLeft = WhichSideIsPlayer();


            if (Vector3.Distance(playerPos, enemyTransform.position) < attackRange)
            {
                isAttacking = true;
                enemyRigidBody2D.velocity = Vector2.zero;
                //BulkyAttack(PlayerIsLeft);
                //print("Attack"); //Not quite sure how we will be implementing attack?
            }
            else if (!isAttacking)
            {
                if (PlayerIsLeft)
                {
                    enemyRigidBody2D.AddForce(Vector2.left * 60 * EnemySpeed * Time.deltaTime);
                }
                else
                {
                    enemyRigidBody2D.AddForce(Vector2.right * 60 * EnemySpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            enemyRigidBody2D.velocity = Vector2.zero;
        }

        if (PlayerIsLeft)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;
        //animator.SetBool("IsPlayerLeft", PlayerIsLeft);
        animator.SetBool("Attacking", isAttacking);
    }
    
    public override void TakeDamage(float baseDamage)
    {
        animator.SetTrigger("Damaged");
        StartCoroutine(BlinkRed());
        base.TakeDamage(baseDamage);
    }

    IEnumerator BlinkRed()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _spriteRenderer.color = Color.white;
    }

    public override void OnDeath()
    {
        animator.SetBool("Dead", true);
        GameObject.Destroy(this.gameObject, 3);
    }
}
