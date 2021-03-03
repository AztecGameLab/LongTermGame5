using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : ProjectileWeapon
{
    
    public Transform FirePoint;
    public GameObject fireBall;
    
    public float launchForce;

    private float fireBallSize = 3f;
    public float chargeTimer;
    public KeyCode charge;
    
   

    public override void Charge()
    {
        
    }
    public override void Fire()
    {
        
        GameObject newFireBall = Instantiate(fireBall, FirePoint.position, FirePoint.rotation);
        
        fireBallSize = 3f;
        chargeTimer = 0f;
    }
}
