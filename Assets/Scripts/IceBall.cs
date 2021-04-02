using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") //incase the spawn position of the ice ball overlaps with the player
        {
            Vector3 freeze = collision.gameObject.transform.position; // gets the position of the object it hit
            collision.gameObject.transform.position = freeze; // "freezes" the gameobject in the position they were hit

            Destroy(gameObject);
        }
    }
}