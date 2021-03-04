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
    public float recoil;
    public float upForce;
    public float chargeTimer;
    private float FireBallSize = 3f;
   
    public override void Fire()
    {
        GameObject newFireBall = Instantiate(fireBall, firePoint.position, firePoint.rotation);
        FireBallSize += chargeTimer;
        Debug.Log(FireBallSize);
        newFireBall.transform.localScale = new Vector3(FireBallSize, FireBallSize, FireBallSize);
        newFireBall.GetComponent<Rigidbody2D>().velocity = (newFireBall.transform.right * launchForce) + (newFireBall.transform.up * upForce);
        chargeTimer = 0f;
        FireBallSize = 3f;
    }

    public override void Charge()
    {
        if (chargeTimer < 2)
        {
            chargeTimer += Time.deltaTime;
            
            recoil += 1;
            
        }
        
        
        
    }

}
