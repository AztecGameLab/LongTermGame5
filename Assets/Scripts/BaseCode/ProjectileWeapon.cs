using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileWeapon : ScriptableObject
{
    
    public virtual void Fire(Vector2 direction){}
    public virtual void Charge(Vector2 direction){}
    public virtual void OnAimChange(Vector2 direction){}
}
