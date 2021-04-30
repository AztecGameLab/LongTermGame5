using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileWeapon : ScriptableObject
{
    //These are deprecated!!
    public virtual void Fire(){}
    public virtual void Charge(){}

    public Sprite RuneSprite;
    public float manaCost;

    public virtual void Cancel(){}

    public virtual void Fire(Vector2 direction){}
    public virtual void Charge(Vector2 direction){}
    public virtual void OnAimChange(Vector2 direction){}
}
