using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class GroundPoundDestructable : Entity
{
    public override void TakeDamage(float baseDamage)
    {
        if (hasDamageSound && health <= 0)
            RuntimeManager.PlayOneShotAttached(entityDeathSound, gameObject);
        else if (hasDamageSound && health > 0)
            RuntimeManager.PlayOneShotAttached(entityDamageSound, gameObject);
        

    }

    public void GroundPoundTakeDamage(float damage)
    {
        TakeDamage(damage);
        
        health -= damage;

        if(health <= 0)
            OnDeath();
    }
    
}
