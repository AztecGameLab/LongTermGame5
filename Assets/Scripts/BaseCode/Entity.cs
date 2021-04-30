using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using FMODUnity;
using UnityEngine;

/// <summary>
/// This is just a simple base for any damagable objects
/// </summary>
public class Entity : MonoBehaviour
{
    [SerializeField]
    public float health;

    [SerializeField, EventRef] private string entityDamageSound = "event:/Player/Injury/Generic Damage";
    [SerializeField, EventRef] private string entityDeathSound = "event:/Player/Death Sound/Death Plop";
    [SerializeField] protected bool hasDamageSound = false;
    
    [EasyButtons.Button]
    public virtual void TakeDamage(float baseDamage)
    {
        if (hasDamageSound && health <= 0)
            RuntimeManager.PlayOneShotAttached(entityDeathSound, gameObject);
        else if (hasDamageSound && health > 0)
            RuntimeManager.PlayOneShotAttached(entityDamageSound, gameObject);
        
        health -= baseDamage;

        if(health <= 0)
            OnDeath();
    }

    //Direction from what dealt the damage to the entity
    // damage dealer --> entity
    
    //The direction should NOT be normalized
    //meaning it can also handle the intensity
    public virtual void TakeDamage(float baseDamage, Vector2 direction){
        TakeDamage(baseDamage);
    }

    public virtual void OnDeath(){
        //AAAAAA I'm Dying!!! 💀
        Destroy(gameObject, 0);
    }
}
