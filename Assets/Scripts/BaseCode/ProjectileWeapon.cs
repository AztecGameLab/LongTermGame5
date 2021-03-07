using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : ScriptableObject
{
    [Tooltip("This is how much mana each fire of this weapon uses")]
    public float ManaCost = 1;

    protected PlatformerController controller;

    public void ApplyKnockBack(float intensity){
        
    }

    public virtual void Fire(Vector2 direction){}
    public virtual void Charge(Vector2 direction){}
}
