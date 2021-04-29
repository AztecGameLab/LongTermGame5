using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulkyEnemy : Entity
{
    private float healthTrigger;
    private Rigidbody2D enemyRigidBody2D;
    private Transform enemyTransform;
    public float EnemySpeed;
    private float attackRange = 2;
    public bool lowHealth;
    private bool isAttacking, PlayerIsLeft;
    public Animator animator;
    private SpriteRenderer _spriteRenderer;
    public float damage = 1;
    
    private PlatformerController _player;
    private Vector3 PlayerPosition => _player.transform.position;
    
    public void Awake()
    {
        attackRange = 3;
        enemyRigidBody2D = GetComponent<Rigidbody2D>();
        enemyTransform = GetComponent<Transform>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _player = PlatformerController.instance;
        lowHealth = false;
        healthTrigger = health / 2;
        isAttacking = false;
        PlayerIsLeft = false;
        animator.SetBool("Walking", true);
        GetComponentInChildren<BulkyEnemyEvent>().BulkyEnemy = this;
    }

    public void AttackEnd()
    {
        var hits = Physics2D.CircleCastAll(transform.position, 1, PlayerIsLeft ? Vector2.left : Vector2.right, attackRange);
        foreach (var hit in hits)
        {
            if(hit.transform.CompareTag("Player"))
                _player.TakeDamage(damage);
        }
        isAttacking = false;
    }

    private bool WhichSideIsPlayer()
    {
        return enemyRigidBody2D.position.x >= PlayerPosition.x;
    }
    
    private void Update()
    {
        if (_player.isDying)
        {
            enabled = false;
            return;
        }
        
        if (enabled)
        {
            if (!lowHealth && health < healthTrigger)
            {
                lowHealth = true;
                EnemySpeed *= 2;
            }

            PlayerIsLeft = WhichSideIsPlayer();

            if (Vector3.Distance(PlayerPosition, enemyTransform.position) < attackRange)
            {
                isAttacking = true;
                enemyRigidBody2D.velocity = Vector2.zero;
            }
            else if (!isAttacking)
            {
                if (PlayerIsLeft)
                {
                    enemyRigidBody2D.AddForce(Vector2.left * (60 * EnemySpeed * Time.deltaTime));
                }
                else
                {
                    enemyRigidBody2D.AddForce(Vector2.right * (60 * EnemySpeed * Time.deltaTime));
                }
            }
        }
        else
        {
            enemyRigidBody2D.velocity = Vector2.zero;
        }

        _spriteRenderer.flipX = PlayerIsLeft;
        animator.SetBool("Attacking", isAttacking);
    }
    
    public override void TakeDamage(float baseDamage)
    {
        animator.SetTrigger("Damaged");
        StartCoroutine(BlinkRed());
        base.TakeDamage(baseDamage);
    }

    private IEnumerator BlinkRed()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _spriteRenderer.color = Color.white;
    }

    public override void OnDeath()
    {
        animator.SetBool("Dead", true);
        Destroy(gameObject, 3);
    }
}
