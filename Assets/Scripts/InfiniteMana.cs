using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************************************************************************
 * Script is meant to be applied to a Infinite Mana Spawner
 * Inherits the entity class in order for the object to appear to take damage
 * When TakeDamage is called, 1 - 3 mana orbs will spawn which can be picked up by the player
 * Spawner will always regen back to full health which makes it indestructible.
*********************************************************************************************/
public class InfiniteMana : Entity
{
    public GameObject manaOrb; //Set this to the Mana Orb prefab
    private float maxHealth; 
    private float spawnerCordsX; 
    private float spawnerCordsY;
    private static System.Random rand = new System.Random();

    public override void TakeDamage(float baseDamage)
    {
        base.TakeDamage(baseDamage);

        Vector2 orbPosition;
        for (int i = 0; i < rand.Next(1, 4); ++i) //Generate 1 - 3 mana orbs
        {
            orbPosition = new Vector2(spawnerCordsX + i + 3, spawnerCordsY); //Makes it so that orbs are separated from each other; Testing purposes
            Instantiate(manaOrb, orbPosition, Quaternion.identity); //Creates a new Mana Orb prefab on the scene
        }
        base.health = maxHealth; //Health resets back to full 
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnerCordsX = gameObject.GetComponent<Transform>().position.x;
        spawnerCordsY = gameObject.GetComponent<Transform>().position.y;
        maxHealth = base.health;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            TakeDamage(100);
        }
    }

}
