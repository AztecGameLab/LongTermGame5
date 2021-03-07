using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private float poke;
    [SerializeField] private float timer;
    [SerializeField] private float damage;
    private float smoothing = 7f;

    private PolygonCollider2D poly;

    private void Awake() {
        poly = this.GetComponent<PolygonCollider2D>();
        poly = this.gameObject.AddComponent<PolygonCollider2D>();
        poly.isTrigger = true;
    }
    private void Start() {
        poke = 5f;
        damage = 1f;
        timer = 0.3f;
    }
    private void OnTriggerEnter2D(Collider2D other) {

        Rigidbody2D person = other.GetComponent<Rigidbody2D>();
        Entity entity = other.GetComponent<Entity>();

        if (entity != null) {
            entity.TakeDamage(damage); // Player takes damage.
            Debug.Log(entity.health);
        }

        if (person != null) {
            person.gravityScale = 0;
            Vector2 direction = person.transform.position - transform.position;
            //int coor = other.GetContacts;
            direction = direction.normalized * poke;
            Debug.Log(direction);
            StartCoroutine(Knocked(direction, person));
            person.AddForce(direction, ForceMode2D.Impulse);
        }
        
    }
    private IEnumerator Knocked(Vector2 direction, Rigidbody2D person) {
        if(person != null) {
            yield return new WaitForSeconds(timer);
            person.velocity = Vector2.zero;
            person.gravityScale = 1;
        }
    }

}
