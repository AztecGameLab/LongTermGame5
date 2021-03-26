using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireBall", menuName = "FireBall")]
public class FireBallStats : ProjectileWeapon
{
    public GameObject fireBall;
    GameObject newFireBall;
    public Transform firePoint;
    private Vector2 spawn;

    public float launchForce;
    public float recoil;
    public float upForce;
    public float chargeTimer;
    private float posNeg;

    private float damage;
    private float FireBallSize = 3f;
    public GameObject player;
    

    public override void Fire(Vector2 direction)
    {
        if (direction.x > 0)
            posNeg = 1;
        if (direction.x < 0)
            posNeg = -1;
        player = GameObject.Find("TempPlayer");
        spawn = player.GetComponent<Transform>().position;
        spawn.x += posNeg;
        fireBall.GetComponent<Transform>().position = spawn;
        newFireBall = Instantiate(fireBall, fireBall.transform.position, fireBall.transform.rotation);
        FireBallSize += chargeTimer;
        
        newFireBall.transform.localScale = new Vector3(FireBallSize, FireBallSize, FireBallSize);
        
        newFireBall.GetComponent<Rigidbody2D>().velocity = (newFireBall.transform.right * posNeg * launchForce) + (newFireBall.transform.up * upForce);
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
