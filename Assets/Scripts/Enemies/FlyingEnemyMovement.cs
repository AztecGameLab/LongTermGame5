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

    bool IsAttacking = false;
    bool CanMove = true;
    float rotate = 180;
    Rigidbody2D rb;
    
    

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlatformerController>();
        rb = GetComponent<Rigidbody2D>();
        PeacefulPattern();
    }

    // Update is called once per frame
    void Update()
    {

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

    private void LookatPlayer()
    {
        Vector3 rotation = Quaternion.LookRotation(player.transform.position).eulerAngles;
        rotation.y = 0;
        rotation.x = 0;
        transform.rotation = Quaternion.Euler(rotation);
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
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlatformerController>())
        {
            IsAttacking = false;
            PeacefulPattern();
        }
    }

}
