using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***********************************************************
 * Script will change an object's texture to appear damage
 * Accomplished by changing Sprites when object is within a
 * certain range (Health calculated in percent)
 ***********************************************************/
public class Destructible : Entity
{
    public Sprite[] Sprites; //The different textures go into this array
    private float[] HealthPercents;
    private float MaxHealth; //Constant value; an objects maximum health
    private float prevHealth;

    //Private method determines the cutoffs on percent the ranges based on the number of Sprites
    private float[] CreatePercents(int numSprites)
    {
        float[] HealthPercents = new float[numSprites];

        HealthPercents[0] = 100;
        if (numSprites > 1)
        {
            float DISTANCE = 100 / (numSprites - 1); //Constant value; Distance from one cutoff to another
            for (int i = 1; i < numSprites; ++i)
            {
                HealthPercents[i] = HealthPercents[i - 1] - DISTANCE;
            }
        }
        return HealthPercents;
    }

    //Private method which will gauge what texture to display when a health percentage is within a desired range
    private void ChangeTextures()
    {
        float currPercent;

        for (int i = 0; i < HealthPercents.Length - 1; ++i)
        {
            currPercent = base.health / MaxHealth * 100;

            if (currPercent < HealthPercents[i] & currPercent >= HealthPercents[i + 1])
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = Sprites[i + 1];
                break;
            }
        }
    }

    //Overrides the OnDeath method inherited from the Entity class
    public override void OnDeath()
    {
        GameObject.Destroy(this.gameObject, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = base.health;
        prevHealth = base.health;
        HealthPercents = CreatePercents(Sprites.Length);
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (base.health <= 0)
        {
            OnDeath();
            return;
        } 

        if (base.health < prevHealth) //Now will only update if Object has lost health
        {
            ChangeTextures();
        }

        prevHealth = base.health;
    }
}
