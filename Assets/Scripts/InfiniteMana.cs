using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

/*********************************************************************************************
 * Script is meant to be applied to a Infinite Mana Spawner
 * Inherits the entity class in order for the object to appear to take damage
 * When TakeDamage is called, 1 - 3 mana orbs will spawn which can be picked up by the player
 * Spawner will always regen back to full health which makes it indestructible.
*********************************************************************************************/
public class InfiniteMana : Entity
{
    public GameObject manaOrb; //Set this to the Mana Orb prefab
    [EventRef] public string manaHitSound;
    
    public override void TakeDamage(float baseDamage)
    {
        for (int i = 0; i < Random.Range(1, 4); ++i) //Generate 1 - 3 mana orbs; Random.Range max is exclusive 
        {
            Vector2 orbPosition = Random.insideUnitCircle + Vector2.up * 2;
            // orbPosition.x += transform.position.x; //These must be added to the orb position in order to spawn around spawner
            // orbPosition.y += transform.position.y;
            var go = Instantiate(manaOrb, (Vector2) transform.position + orbPosition, Quaternion.identity); //Creates a new Mana Orb prefab on the scene
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5f, 5f), 5);
            RuntimeManager.PlayOneShot(manaHitSound, transform.position);
        }
    }
}