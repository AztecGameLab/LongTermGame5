
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEnemyScript : Entity
{
    [SerializeField] Transform player;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float rushDistance = 2;
    //public static bool passive = true; //use this when making all enemies aggressive
    [SerializeField] bool passive; //used for testing
    bool onCooldown = false;
    Vector3 difference;
    bool inRange;
    float rushSpeed;
    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
            ChasePlayer();
        }
        else
        {
            Look(); //looks at player from a stationary POV
        }
    }

    public void changePasive() //change the passive based on the player's skill unlocks        /\/\/\/\TODO/\/\/\/\
    {
        //if([skill unlocked]){ passive = false;}
    }

    public float getDistance()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        return distance;
    }

    private void ChasePlayer()
    {

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
        if(transform.position.x < player.position.x) //faces right if player is on left side
        {
            transform.localScale = new Vector2(1, 1);
        }
        else                                        //faces left if player is on right side
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }


    float freezeTime = 3;
    float knockForce = 4;
    private void Attack()//implement player's take damage function
    {
        if (!passive)
        {
            float temp = PlatformerController.instance.parameters.KnockBackTime;
            PlatformerController.instance.parameters.KnockBackTime = freezeTime;

            PlatformerController.instance.TakeDamage(15, difference*knockForce);

            PlatformerController.instance.parameters.KnockBackTime = temp;
        }
    }

    void OnTriggerEnter2D(Collider2D col) 
    {
        
        if (col.GetComponent<PlatformerController>())
        {
            difference = (col.transform.position - transform.position).normalized;

            Attack();
            freezePlayer();

        }
    }

    //freezes player for (x) seconds when within range
    private void freezePlayer()
    {
        StartCoroutine("freeze");
        onCooldown = true;

    }

    IEnumerator freeze() //works
    {
        
            if (!onCooldown)
            {
                PlatformerController.instance.lockControls = true;
                Debug.Log("player frozen");
                yield return new WaitForSeconds(3);
                yield return StartCoroutine("cooldown");

            }
        
    }

    IEnumerator cooldown()
    {
       
        if (onCooldown)
          {
            PlatformerController.instance.lockControls = false;
            Debug.Log("freeze attack on cooldown");
            yield return new WaitForSeconds(3);
            onCooldown = false;    
          }
    }

}
