
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEnemyScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float moveSpeed;
    [SerializeField] float rushDistance;
    public static bool passive = false;
    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
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
            //TODO: (maybe) wander()
            //Wander();
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
            moveSpeed = 3;
        }
    }

    public void RushLeft()
    {
        float totalDistance = Math.Abs(getDistance());
        if (totalDistance > -rushDistance) //rushes player when close enough (if on right)
        {
            moveSpeed = 3;
        }
    }

}
