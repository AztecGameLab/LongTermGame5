using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private Vector2 poke;
    [SerializeField] private float spikeDamage;
    [SerializeField] private float timer;

    private PolygonCollider2D poly;

    void Awake() {
        poly = GetComponent<PolygonCollider2D>();
        poly = gameObject.AddComponent<PolygonCollider2D>();
        poke = new Vector2(3f, 3f);
        spikeDamage = 1f;
        timer = 0.1f;
    }
    void OnCollisionEnter2D(Collision2D other) {   
        if (other.gameObject.CompareTag("Player")) {

            Rigidbody2D person = other.rigidbody;
            ContactPoint2D contacted = other.GetContact(0);
            Vector2 playerPos = other.transform.position;
            Vector2 direction =  playerPos - contacted.point;
            direction = direction.normalized * poke;
            Debug.Log(direction);
            person.AddForce(direction, ForceMode2D.Impulse);

            Entity entity = person.GetComponentInParent<Entity>();
            Debug.Log(entity);
            entity.TakeDamage(spikeDamage);


        }
    }
}
