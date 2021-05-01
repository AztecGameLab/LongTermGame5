using System.Collections;
using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PassiveEnemyScript : Entity
{
    Transform player;

    [SerializeField, EventRef] private string hitSound = "event:/Enemies/Water Enemy/Water Enemy Melee Hit";
    
    [SerializeField]
    private float moveSpeed = 1; //tehee hardcoded values

    [SerializeField]
    private float rushDistance = 2;

    public static bool passive = true;
    bool onCooldown = false;
    Vector3 difference;
    bool inRange;
    float rushSpeed;
    Rigidbody2D rb2d;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        player = PlatformerController.instance.transform;
        StartCoroutine(auraLoop());
        Look(); //looks at player from a stationary POV
    }

    // Update is called once per frame
    void Update()
    {
        //distance to player
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        //check if passive
        if (passive == false)
        {
            if (Vector2.Distance(player.position, transform.position) < 15)
                ChasePlayer();
            else
                _animator.SetBool("walking", false);
        }
        else
        {
            Look(); //looks at player from a stationary POV
        }
    }

    [EasyButtons.Button]
    public static void changePassive() //change the passive based on the player's skill unlock
    {
        passive = false;
    }

    public float getDistance()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        return distance;
    }

    private void ChasePlayer()
    {
        if (health <= 0)
            return;
        _animator.SetBool("walking", true);

        if (transform.position.x < player.position.x)
        {
            if (getDistance() < rushDistance) //rushes player when close enough
            {
                RushRight();
            }

            //enemy to left, move right
            rb2d.velocity = new Vector2(moveSpeed, -1);
            //turn enemy right if player is left
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            if (-getDistance() > -rushDistance) //rushes player when close enough
            {
                RushLeft();
            }

            //enemy to right, move left
            rb2d.velocity = new Vector2(-moveSpeed, -1);
            //turn enemy left if player is right 
            transform.localScale = new Vector2(-1, 1);
        }
    }

    public void RushRight()
    {
        float totalDistance = Math.Abs(getDistance());
        if (totalDistance < rushDistance) //rushes player when close enough (if on left)
        {
            moveSpeed = 1.2f;
        }
    }

    public void RushLeft()
    {
        float totalDistance = Math.Abs(getDistance());
        if (totalDistance > -rushDistance) //rushes player when close enough (if on right)
        {
            moveSpeed = 1.2f;
        }
    }

    private void Look()
    {
        if (transform.position.x < player.position.x) //faces right if player is on left side
        {
            transform.localScale = new Vector2(1, 1);
        }
        else //faces left if player is on right side
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }


    float freezeTime = 3;
    float knockForce = 4;

    private void Attack() //implement player's take damage function
    {
        if (!passive)
        {
            if (health <= 0)
                return;
            
            RuntimeManager.PlayOneShotAttached(hitSound, gameObject);
            _animator.Play("slash");
            float temp = PlatformerController.instance.parameters.KnockBackTime; //you told me to do this scuffed af solution Jacob you better not deny the pull req
            PlatformerController.instance.parameters.KnockBackTime = freezeTime;

            PlatformerController.instance.TakeDamage(3, difference * knockForce);

            PlatformerController.instance.parameters.KnockBackTime = temp;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlatformerController>())
        {
            difference = (col.transform.position - transform.position).normalized;

            freezePlayer();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<PlatformerController>())
            Attack();
    }

    //freezes player for (x) seconds when within range
    private void freezePlayer()
    {
        PlatformerController.instance.gameObject.AddComponent<PlayerFreeze>();
        //StartCoroutine("freeze");
        onCooldown = true;
    }

    // private GameObject playerIce;
    //
    // IEnumerator freeze() //works
    // {
    //     if (!onCooldown)
    //     {
    //         PlatformerController.instance.lockControls = true;
    //
    //         var sr = PlatformerController.instance.GetComponent<SpriteRenderer>();
    //         playerIce = Instantiate(Resources.Load("PlayerIce") as GameObject, sr.bounds.center, Quaternion.identity);
    //         playerIce.transform.parent = PlatformerController.instance.transform;
    //         playerIce.transform.localScale = Vector3.one * sr.bounds.size.y;
    //
    //         Debug.Log("player frozen");
    //         yield return new WaitForSeconds(3);
    //         yield return StartCoroutine("cooldown");
    //     }
    // }
    //
    // IEnumerator cooldown()
    // {
    //     if (onCooldown)
    //     {
    //         Destroy(playerIce);
    //         PlatformerController.instance.lockControls = false;
    //         Debug.Log("freeze attack on cooldown");
    //         yield return new WaitForSeconds(3);
    //         onCooldown = false;
    //     }
    // }

    public SpriteRenderer aura;
    public Collider2D auraTrigger;
    IEnumerator auraLoop()
    {
        while (health > 0)
        {
            aura.enabled = false;
            auraTrigger.enabled = false;

            yield return new WaitForSeconds(2);
            
            aura.enabled = true;
            auraTrigger.enabled = true;

            yield return new WaitForSeconds(3);
            
            
            yield return null;
        }
    }

    public override void TakeDamage(float baseDamage)
    {
        if (health <= 0)
            return;
        _animator.Play("damage");
        base.TakeDamage(baseDamage);
    }

    public override void OnDeath()
    {
        foreach (Transform t in transform)
        {
            gameObject.layer = 16;
        }
        _animator.Play("death");
        GameObject.Destroy(this.gameObject, 3);
    }
}