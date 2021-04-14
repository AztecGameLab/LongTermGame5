using UnityEngine;

/***********************************************************
 * Script will change an object's texture to appear damage
 * Accomplished by changing Sprites when object is within a
 * certain range (Health calculated in percent)
 ***********************************************************/
public class Destructible : Entity
{
    [SerializeField] private Sprite[] sprites; //The different textures go into this array
    [SerializeField] private EntityData entityData;
    
    private float[] _healthPercents;
    private SpriteRenderer _spriteRenderer; 

    private void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprites[0];

        _healthPercents = CreatePercents(sprites.Length);
    }
    
    //Private method determines the cutoffs on percent the ranges based on the number of Sprites
    private static float[] CreatePercents(int numSprites)
    {
        float[] healthPercents = new float[numSprites];

        healthPercents[0] = 100;
        if (numSprites > 1)
        {
            float distance = 100 / (numSprites - 1); //Constant value; Distance from one cutoff to another
            for (int i = 1; i < numSprites; ++i)
            {
                healthPercents[i] = healthPercents[i - 1] - distance;
            }
        }
        return healthPercents;
    }
    
    //Overrides TakeDamage from entity class; will also prompt a change of textures when object is damaged
    public override void TakeDamage(float baseDamage)
    {
        base.TakeDamage(baseDamage);
        ChangeTextures();
    }

    //Private method which will gauge what texture to display when a health percentage is within a desired range
    private void ChangeTextures()
    {
        for (int i = 0; i < _healthPercents.Length - 1; ++i)
        {
            var currPercent = health / entityData.MaxHealth * 100;

            if (currPercent < _healthPercents[i] & currPercent >= _healthPercents[i + 1])
            {
                _spriteRenderer.sprite = sprites[i + 1];
                break;
            }
        }
    }
}