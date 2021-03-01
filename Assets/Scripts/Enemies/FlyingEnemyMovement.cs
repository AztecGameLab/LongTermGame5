using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
    PlatformerController player;
    [SerializeField] float speed;
    [SerializeField] float amountToMove;
    [SerializeField] float FireRate = 1f;
    [SerializeField] Transform ProjectileSpawnPoint;
    [SerializeField] GameObject Projectile;



    bool IsAttacking = false;
    bool CanFire = true;
    bool CanMove = true;
    float rotate = 180;
    Rigidbody2D rb;
    

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlatformerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (IsAttacking)
        {
            //1-stop, 2-look at player, 3-start shooting Projectile
            rb.velocity = new Vector2(0, 0);
            //look at player
            Vector3 rotation = Quaternion.LookRotation(player.transform.position).eulerAngles;
            rotation.y = 0;
            rotation.x = 0;
            transform.rotation = Quaternion.Euler(rotation);

            AttackPattern();
        }
        else
        {
            PeacefulPattern();
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
        if (CanFire)
        {
            StartCoroutine(Shoot());
        }

    }
    IEnumerator RLMovment()
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
    IEnumerator Shoot()
    {
        CanFire = false;
        Instantiate(Projectile, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation);
        yield return new WaitForSeconds(FireRate);
        CanFire = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlatformerController>())
        {
            IsAttacking = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlatformerController>())
        {
            IsAttacking = false;
        }
    }

}
