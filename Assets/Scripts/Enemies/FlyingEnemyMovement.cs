using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
    PlatformerController player;
    [SerializeField] private float speed;
    [SerializeField] private float amountToMove;
    [SerializeField] private float FireRate = 1f;
    [SerializeField] private Transform ProjectileSpawnPoint;
    [SerializeField] private GameObject Projectile;
    [SerializeField] float playerDistanceBeforeDashing = .3f;
    [SerializeField] float dashSpeed = 1f;
    [SerializeField] float dashtime = .1f;
    [SerializeField] float dashCoolDown;
    float tempdashCoolDown;
    bool hasDashed = false;
    bool IsAttacking = false;
    bool CanMove = true;
    float rotate = 180;
    
    Rigidbody2D rb;
    
    

    // Start is called before the first frame update
    void Start()
    {

        player = PlatformerController.instance;
        rb = GetComponent<Rigidbody2D>();
        PeacefulPattern();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (IsAttacking)
        {
            Vector3 lookDirection = player.transform.position - transform.position;
            LookatPlayer();
            if (lookDirection.x > -playerDistanceBeforeDashing && lookDirection.x < playerDistanceBeforeDashing)
            {
                StartCoroutine(Dash(lookDirection));
            }
            
        }
    }

    private void PeacefulPattern()
    {

        if (CanMove)
        {
            StartCoroutine(RLMovment());
        }
        
    }

    private void AttackPattern()
    {
        //1-stop, 2-look at player, 3-start shooting Projectile
        rb.velocity = new Vector2(0, 0);
        LookatPlayer();
        StartCoroutine(Shoot());

        
    }
    IEnumerator Dash(Vector2 dir)
    {
        if (!hasDashed)
        {

            if (dashCoolDown != tempdashCoolDown)
            {
                
                hasDashed = true;
                // only dash 10% higher
                dir.y *= .1f;
                rb.AddForce(-dir.normalized * dashSpeed);
                yield return new WaitForSeconds(dashtime);
                rb.velocity = new Vector2(0, 0);
                hasDashed = false;
                tempdashCoolDown = dashCoolDown;

            }
            else
            {
                hasDashed = true;
                yield return new WaitForSeconds(dashCoolDown);
                tempdashCoolDown = 0;
                hasDashed = false;

            }
        }



    }

    private void LookatPlayer()
    {
        float rotate = 180;
        if (transform.position.x > player.transform.position.x)
        {
            rotate = 0;
        }
        else if (transform.position.x < player.transform.position.x)
        {
            rotate = 180;
        }
        transform.localRotation = Quaternion.Euler(0, rotate, 0);
    }

    IEnumerator RLMovment()
    {
        while (!IsAttacking)
        {
            CanMove = false;
            //flip right to left by adding nagative velocity

            rotate += 180;
            speed *= -1f;
            transform.localRotation = Quaternion.Euler(0, rotate, 0);
            rb.velocity = new Vector2(speed, 0);
            yield return new WaitForSeconds(amountToMove);
            CanMove = true;
        }
    }
    IEnumerator Shoot()
    {
        while (IsAttacking)
        {
            LookatPlayer();
            Instantiate(Projectile, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation);
            yield return new WaitForSeconds(FireRate);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlatformerController>())
        {
            IsAttacking = true;
            AttackPattern();
            GetComponent<CircleCollider2D>().radius *= 2f;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlatformerController>())
        {
            IsAttacking = false;
            PeacefulPattern();
            GetComponent<CircleCollider2D>().radius /= 2f;
        }
    }

}
