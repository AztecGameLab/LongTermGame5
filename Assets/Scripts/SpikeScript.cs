using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private float poke;
    [SerializeField] private float timer;
    public PolygonCollider2D poly;


    /***********************************************************************************************
     * SpikeScript 
     **********************************************************************************************/
    private void Awake() {
        poly = this.GetComponent<PolygonCollider2D>();
        poly = this.gameObject.AddComponent<PolygonCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        poke = 5f;
        timer = 0.5f;

        Entity entity = other.GetComponent<Entity>();
        if (entity != null) {
            entity.TakeDamage(1f); // Player takes damage.
            Debug.Log(entity.health);
        }
        Rigidbody2D person = other.GetComponent<Rigidbody2D>();
        if (person != null) {
            Vector2 direction = person.transform.position - transform.position;
            //Vector2 direction = other.GetContacts() - GetContacts;
            direction = direction.normalized * poke;
            person.AddForce(direction, ForceMode2D.Impulse);
            if(direction.x < 0) {
                //Debug.Log("Move transform Left");
                //while (timer > 0) {
                //    person.AddForce(new Vector3(-20f, 1f, 0f));
                //    timer -= Time.deltaTime;
                //}
                // move on x-axis to the left
            } else {
                //Debug.Log("Move transform right");
                //while (timer > 0){
                //    person.AddForce(new Vector3(20f,1f,0f));
                //    timer -= Time.deltaTime;
                //}
                // move on x-axis to the right
            }
        }
    }


}
