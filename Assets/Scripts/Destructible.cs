﻿using UnityEngine;

/***********************************************************
 * Script will change an object's texture to appear damage
 * Accomplished by changing Sprites when object is within a
 * certain range (Health calculated in percent)
 ***********************************************************/
public class Destructible : Entity
{
    [SerializeField] private Sprite[] sprites; //The different textures go into this array
    [SerializeField] private bool destroyOnEnd = true;
    [SerializeField] private EntityData entityData;
    
    private float[] _healthPercents;
    private SpriteRenderer _spriteRenderer; 

    private void Start()
    {
        if (health > entityData.MaxHealth)
            Debug.LogWarning($"Tried to set health ({health}) greater than the MaxHealth ({entityData.MaxHealth}) for {entityData.name}");
        
        health = Mathf.Clamp(health, 0, entityData.MaxHealth);
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprites[0];
        _healthPercents = CreatePercents(sprites.Length);
    }
    
    // Determines the cutoffs on percent the ranges based on the number of Sprites
    private static float[] CreatePercents(int numSprites)
    {
        float[] healthPercents = new float[numSprites];
        float frequencyOfChange = 1 / ((float) numSprites - 1);
        
        for (int i = 0; i < healthPercents.Length; i++)
            healthPercents[i] = 1 - i * frequencyOfChange;
        
        return healthPercents;
    }
    
    public override void TakeDamage(float damage)
    {
        if (health <= 0 && !destroyOnEnd)
            return;
        
        base.TakeDamage(damage);
        ChangeTextures();
    }

    // Gauges what texture to display when a health percentage is within a desired range
    private void ChangeTextures()
    {
        var curPercent = health / entityData.MaxHealth;

        for (int i = 0; i < _healthPercents.Length - 1; ++i)
        {
            if (curPercent < _healthPercents[i] && curPercent >= _healthPercents[i + 1])
            {
                _spriteRenderer.sprite = sprites[i + 1];
                break;
            }
        }
    }

    public override void OnDeath()
    {
        if (destroyOnEnd)
            base.OnDeath();
    }
}