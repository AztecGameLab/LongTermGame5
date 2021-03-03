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
    public Sprite[] sprites; //The different textures go into this array
    private float[] healthPercents;
    private float maxHealth; //Constant value; an objects maximum health
    private SpriteRenderer spriteRenderer; 

    //Private method determines the cutoffs on percent the ranges based on the number of Sprites
    private float[] CreatePercents(int numSprites)
    {
        float[] HealthPercents = new float[numSprites];

        HealthPercents[0] = 100;
        if (numSprites > 1)
        {
            float distance = 100 / (numSprites - 1); //Constant value; Distance from one cutoff to another
            for (int i = 1; i < numSprites; ++i)
            {
                HealthPercents[i] = HealthPercents[i - 1] - distance;
            }
        }
        return HealthPercents;
    }

    //Private method which will gauge what texture to display when a health percentage is within a desired range
    private void ChangeTextures()
    {
        float currPercent;

        for (int i = 0; i < healthPercents.Length - 1; ++i)
        {
            currPercent = base.health / maxHealth * 100;

            if (currPercent < healthPercents[i] & currPercent >= healthPercents[i + 1])
            {
                spriteRenderer.sprite = sprites[i + 1];
                break;
            }
        }
    }
    
    //Overrides TakeDamage from entity class; will also prompt a change of textures when object is damaged
    public override void TakeDamage(float baseDamage)
    {
        base.TakeDamage(baseDamage);
        ChangeTextures();
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        healthPercents = CreatePercents(sprites.Length);
        spriteRenderer.sprite = sprites[0];
        maxHealth = base.health;
    }

}
