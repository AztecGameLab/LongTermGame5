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
        poke = 1f;
        damage = 1f;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        timer = 1f;

        Rigidbody2D person = other.GetComponent<Rigidbody2D>();
        Entity entity = other.GetComponent<Entity>();
        poly.isTrigger = false;
        if (entity != null) {
            entity.TakeDamage(damage); // Player takes damage.
            Debug.Log(entity.health);
        }

        if (person != null) {
            Vector2 direction = person.transform.position - transform.position;
            //int coor = other.GetContacts;
            direction = direction.normalized * poke;
            Debug.Log(direction);
            Vector2 target = (Vector2)person.transform.position + direction;
            Debug.Log("Player pos"+ person.transform.position);
            Debug.Log("Target pos"+ target);
            //person.AddForce(direction, ForceMode2D.Impulse);
            //person.velocity = new Vector2(-50f, person.velocity.y);
            StartCoroutine(Knocked(person, other, target));
            if (direction.x < 0) {
                //Debug.Log("Move transform Left");
                //while (timer > 0) {
                //   person.velocity = new Vector2(-50f,person.velocity.y);
                //   timer -= Time.deltaTime;
                //}
                // move on x-axis to the left
                
            } else {
                //Debug.Log("Move transform right");
                //while (timer > 0){
                //    person.velocity = new Vector2(50f, person.velocity.y);
                //    timer -= Time.deltaTime;
                //}
                // move on x-axis to the right
            }
        }
        poly.isTrigger = true;
    }

    private IEnumerator Knocked(Rigidbody2D person, Collider2D other,Vector2 target) {
        while(Vector2.Distance(person.transform.position, target) > 0.05f) {
            person.transform.position = Vector2.Lerp(person.transform.position, target, Time.deltaTime/1000);
           
        }
        Debug.Log("End Pos" + person.transform.position);
        yield return null;
    }
}
