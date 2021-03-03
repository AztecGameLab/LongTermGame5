using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireBall", menuName = "FireBall")]
public class FireBallStats : ProjectileWeapon
{
    public GameObject fireBall;
    public Transform firePoint;

    public float launch;
    public float speed;
    
    void Start()
    {
        fireBall = Instantiate(fireBall, firePoint.position, firePoint.rotation);
    }
    public override void Fire()
    {

    }

    public override void Charge()
    {

    }

}
