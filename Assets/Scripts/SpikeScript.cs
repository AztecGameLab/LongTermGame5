using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************************************************************************************************
 * Requires that the player object to have the tag "Player". Each spike will receive a BoxCollider 
 * and is set on awake to a specific offset and size. The player OnCollision with a spike will 
 * always be pushed at a 45 degree angle. Intensity is multiplied by the signed Vector. 
 * Do note, if intensity > 5, then knockback will be greater than the players actual jump height.
 * Also, this script doesnt disable the players jumping ability. So the player can jump and move
 * while being knockedback.
 **************************************************************************************************/
public class SpikeScript : MonoBehaviour
{
    [SerializeField] private float intensity;
    [SerializeField] private float spikeDamage;
    private PolygonCollider2D poly;

    void Awake() {
        poly = gameObject.AddComponent<PolygonCollider2D>();
        poly.autoTiling = true;
        intensity = 5f;
        spikeDamage = 1f;   //TODO :: spikeDamage will probably be greater than 1
    }
    void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.CompareTag("Player")) {
            Vector2 playerPos = other.transform.position;
            ContactPoint2D contacted = other.GetContact(0);  
            Vector2 direction =  playerPos - contacted.point;
            direction = new Vector2(Mathf.Sign(direction.x), Mathf.Sign(direction.y));  // 45 degree 
    
            //Debug.DrawLine(playerPos,playerPos+direction);
            var player = PlatformerController.instance;
            player.TakeDamage(spikeDamage);
            player.KnockBack(direction, intensity);
            //Debug.Log(player.health);
        }
    }
}
