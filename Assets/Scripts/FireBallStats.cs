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
    public int damage;

    private float FireBallSize = 3f;
    public GameObject player;
    

    public override void Fire(Vector2 direction)
    {
        damage = damage / 10;
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
            player.GetComponent<Rigidbody2D>().AddForce(player.transform.right * recoil * posNeg * -5);
        }
        newFireBall.transform.localScale = new Vector3(FireBallSize, FireBallSize, FireBallSize);
        chargeTimer = 0f;
        recoil = 100;
        FireBallSize = 3f;
        damage += 10;
        rocket = false;
    }

    public override void Charge(Vector2 direction)
    {
        if (chargeTimer < 2)
        {
            chargeTimer += Time.deltaTime;

            recoil += 1;
            damage += 1;

        }
    }
    public override void OnAimChange(Vector2 direction) {
        if (direction.x > 0)
            posNeg = 2;
        if (direction.x < 0)
            posNeg = -2;
        if (direction.y < 0)
            rocket = true;
    }
}
