using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private float intensity;
    [SerializeField] private float spikeDamage;

    void Awake() {
        this.gameObject.AddComponent<BoxCollider2D>();
        intensity = 5f;
        spikeDamage = 1f;
    }
    void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.CompareTag("Player")) {

            Vector2 playerPos = other.transform.position;
            ContactPoint2D contacted = other.GetContact(0);
            
            Vector2 direction =  playerPos - contacted.point;
            direction = new Vector2(Mathf.Sign(direction.x), Mathf.Sign(direction.y));  //always push target at a 45 degree angle
            
            Debug.DrawLine(playerPos,playerPos+direction);


            var player = PlatformerController.instance;
            player.KnockBack(direction, intensity);
            player.TakeDamage(spikeDamage);
            
            Debug.Log(direction); 
        }
    }
}
