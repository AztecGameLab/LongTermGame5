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
    private float chargeTimer;
    private float posNeg;
    private bool rocket;

    private float FireBallSize = 3f;
    public GameObject player;
    

    public override void Fire(Vector2 direction)
    {
        player = GameObject.Find("TempPlayer");
        spawn = player.GetComponent<Transform>().position;
     
        spawn.x += posNeg;
        if (rocket)
        {
            spawn.y -= 2;
            spawn.x = player.GetComponent<Transform>().position.x;
            
        }
        fireBall.GetComponent<Transform>().position = spawn;
        newFireBall = Instantiate(fireBall, fireBall.transform.position, fireBall.transform.rotation);
        FireBallSize += chargeTimer;
        if(rocket) {
            newFireBall.GetComponent<Rigidbody2D>().velocity = (newFireBall.transform.up * -1 * launchForce);
            player.GetComponent<Rigidbody2D>().velocity = (player.transform.up * 10);
        }
        else
        {
            newFireBall.GetComponent<Rigidbody2D>().velocity = (newFireBall.transform.right * posNeg * launchForce) + (newFireBall.transform.up * upForce);
        }
        newFireBall.transform.localScale = new Vector3(FireBallSize, FireBallSize, FireBallSize);
        chargeTimer = 0f;
        
        FireBallSize = 3f;
        rocket = false;
    }

    public override void Charge(Vector2 direction)
    {
        if (chargeTimer < 2)
        {
            chargeTimer += Time.deltaTime;

            recoil += 1;

        }
    }
    public override void OnAimChange(Vector2 direction) {
        if (direction.x > 0)
            posNeg = 1;
        if (direction.x < 0)
            posNeg = -1;
        if (direction.y < 0)
            rocket = true;
    }
}
