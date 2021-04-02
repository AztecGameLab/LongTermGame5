
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEnemyScript : Entity
{
    [SerializeField] Transform player;
    [SerializeField] float moveSpeed;
    [SerializeField] float rushDistance;
    //public static bool passive = false; //use this when making all enemies aggressive
    [SerializeField] bool passive; //used for testing
    private bool shouldAttack = false;
    //[SerializeField] bool shouldAttack; //use for testing knockback
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
            Attack();
        }
        else
        {
            Look(); //looks at player from a stationary POV
        }
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
            moveSpeed = 2;
        }
    }

    public void RushLeft()
    {
        float totalDistance = Math.Abs(getDistance());
        if (totalDistance > -rushDistance) //rushes player when close enough (if on right)
        {
            moveSpeed = 2;
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

    private void Attack()//implement player's take damage function
    {
        //invoke player take damage function
        if (shouldAttack == true)
        {
            //player.TakeDamage(damage, direction); //TODO: not right, figure out how to make it take damage
        }
        
    }

    void OnTriggerEnter2D(Collider2D col) //todo: broken, always triggers for some reason
    {
        Debug.Log("Trigger");
        if (col.gameObject.tag == "TempPlayer")
        {
            shouldAttack = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("Outside Trigger");
        if (col.gameObject.tag == "TempPlayer")
        {
            shouldAttack = false;
        }
    }

}
