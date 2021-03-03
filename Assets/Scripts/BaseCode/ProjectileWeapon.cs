using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "FireBallStats", order = 1)]
public class ProjectileWeapon : ScriptableObject
{
    
    public virtual void Fire(){}
    public virtual void Charge(){}
}
