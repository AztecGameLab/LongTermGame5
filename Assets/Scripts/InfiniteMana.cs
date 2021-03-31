using System;
using System.Collections;
using System.Collections.Generic;
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
    public override void TakeDamage(float baseDamage)
    {
        for (int i = 0; i < Random.Range(1, 4); ++i) //Generate 1 - 3 mana orbs; Random.Range max is exclusive 
        {
            Vector2 orbPosition = Random.insideUnitCircle;
            orbPosition.x += transform.position.x; //These must be added to the orb position in order to spawn around spawner
            orbPosition.y += transform.position.y;
            Instantiate(manaOrb, orbPosition, Quaternion.identity); //Creates a new Mana Orb prefab on the scene
        } 
    }

}
