using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireBall", menuName = "FireBall")]
public class FireBallStats : ProjectileWeapon
{
    public GameObject fireBall;
    public Transform firePoint;

    public KeyCode charge;

    public float launchForce;
    public float upForce;
    public float chargeTimer;
   
    public override void Fire()
    {
        GameObject newFireBall = Instantiate(fireBall, firePoint.position, firePoint.rotation);
        newFireBall.GetComponent<Rigidbody2D>().velocity = (newFireBall.transform.right * launchForce) + (newFireBall.transform.up * upForce);
    }

    public override void Charge()
    {

    }

}
