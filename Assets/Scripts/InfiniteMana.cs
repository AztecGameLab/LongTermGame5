using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteMana : Entity
{
    public GameObject manaOrb;
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
            Instantiate(manaOrb, orbPosition, Quaternion.identity);
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
