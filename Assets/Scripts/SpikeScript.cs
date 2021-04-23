using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

/***************************************************************************************************
 * Requires that the player object to have the PlatformerController. The player OnCollision with a 
 * spike will always be pushed at a 45 degree angle. Intensity is the force multiplier. 
 * Do note, if intensity > 5, then knockback will be greater than the players actual jump height.
 * Also, this script doesnt disable the players jumping ability. So the player can jump and move
 * while being knockedback from the center of the spike object.
 **************************************************************************************************/
public class SpikeScript : MonoBehaviour {

    [SerializeField] private float intensity = 5;
    [SerializeField] private float spikeDamage = 1;     //TODO :: spikeDamage will probably be greater than 1
    [EventRef] public string A;

    void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.GetComponent<PlatformerController>()) {

            Vector2 direction = (Vector2)other.transform.position - (Vector2)transform.position;
            direction = new Vector2(Mathf.Sign(direction.x), Mathf.Sign(direction.y));  // 45 degree 
            
            var player = PlatformerController.instance;
            player.TakeDamage(spikeDamage);
            player.KnockBack(direction, intensity);

            RuntimeManager.PlayOneShot(A); /*Play Sound*/
        }
    }
}
