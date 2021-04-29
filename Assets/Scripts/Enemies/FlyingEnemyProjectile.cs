using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyProjectile : MonoBehaviour
{
    [SerializeField] float speed =10f;
    [SerializeField] float Damge = 10f;
    Rigidbody2D rb;
    PlatformerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlatformerController.instance;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce((player.transform.position - transform.position).normalized * speed);

        //destroy itself after 5 seconds
        StartCoroutine(DestroyProjectile());
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
