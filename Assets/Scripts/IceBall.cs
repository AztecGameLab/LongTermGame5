using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag != "Player" && collision.rigidbody != null) {
            collision.gameObject.AddComponent<Freeze>();
        }

        Destroy(gameObject);
    }
}