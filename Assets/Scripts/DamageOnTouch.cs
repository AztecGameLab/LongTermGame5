using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The player takes damage through time intervals when they are
 * standing or touching an environmental hazard
 */

public class DamageOnTouch : MonoBehaviour {
    public float damagePerSec = 1;                                             //declares damage amount
    public float damageDelay = 1f;                                             //time in seconds of damage delay
    public PlatformerController triggeredEntity;
    Coroutine currCoroutine;


     void OnTriggerEnter2D(Collider2D other)                                   //call happens when entity enters trigger
    {
        
        triggeredEntity = other.gameObject.GetComponent<PlatformerController>();
        if (triggeredEntity)                                                   //only runs coroutine if object entered is the player
        {
            currCoroutine = StartCoroutine(doDamage());
        }
        
    }

     void OnTriggerExit2D(Collider2D other)                                    //stops coroutine only when player leaves trigger
    {
        if (triggeredEntity)                                                    
        {
            StopCoroutine(currCoroutine);
        }
    }


    private IEnumerator doDamage() {                                            //player takes damage every 0.5 seconds if they are still on trigger
        while (true)
        {
            triggeredEntity.TakeDamage(damagePerSec,0);                         //entity takes damage and sets direction vector to 0
            yield return new WaitForSeconds(damageDelay);                       //waits 1 second before doing damage again
            
        }
    }
}
