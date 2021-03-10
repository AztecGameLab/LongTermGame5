using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float spikeDamage;

    void Awake() {
        this.gameObject.AddComponent<PolygonCollider2D>();
        //x = 4f;
        //y = 3f;
        x = 20f;
        y = 5f;
        spikeDamage = 1f;
    }
    void OnCollisionEnter2D(Collision2D other) {   
        if (other.gameObject.CompareTag("Player")) {

            Vector2 playerPos = other.transform.position;
            ContactPoint2D contacted = other.GetContact(0);
            Vector2 direction =  playerPos - contacted.point;
            direction = direction.normalized * new Vector2(x, y);

            Rigidbody2D player = other.rigidbody;
            //player.AddForce(direction, ForceMode2D.Impulse);

            player.velocity = direction;
            Entity entity = player.GetComponentInParent<Entity>();
            entity.TakeDamage(spikeDamage);
        }
    }
}
