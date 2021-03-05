using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteMana : Entity
{
    private float maxHealth;
    private static System.Random rand = new System.Random();
    public override void TakeDamage(float baseDamage)
    {
        base.TakeDamage(baseDamage);
        for (int i = 0; i < rand.Next(1, 4); ++i) //Generate 1 - 3 mana orbs
        {
            //TODO: Instantiate mana prefabs
        }
        base.health = maxHealth; //Health resets back to full 
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = base.health;
    }

}
