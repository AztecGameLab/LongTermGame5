using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is just a simple base for any damagable objects
/// </summary>
public class Entity : MonoBehaviour
{
    [SerializeField]
    public float health;

    [EasyButtons.Button]
    public virtual void TakeDamage(float baseDamage){
        health -= baseDamage;
        if(health <= 0){
            OnDeath();
        }
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
        GameObject.Destroy(this.gameObject, 0);
    }
}
