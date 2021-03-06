using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private float poke;
    public PolygonCollider2D poly;

    private void Awake() {
        poly = this.GetComponent<PolygonCollider2D>();
        poly = this.gameObject.AddComponent<PolygonCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        poke = 5f;
        Debug.Log("ouch a spike...");   // Player takes damage.
        Rigidbody2D person = other.GetComponent<Rigidbody2D>();
        if(person != null) {
            Vector2 direction = person.transform.position - transform.position;
            direction = direction.normalized * poke;
            person.AddForce(direction, ForceMode2D.Impulse);
        }
    }
}
