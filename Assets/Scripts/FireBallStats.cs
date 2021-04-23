using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireBall", menuName = "FireBall")]
public class FireBallStats : ProjectileWeapon
{
    public GameObject fireBall;
    GameObject newFireBall;
   
    public float launchForce;
    public float recoil;
    public float upForce;
    public float chargeTimer;
    public float damage;
    public float FireBallSize = 3f;
    public Vector2 getDir;
    private Vector2 direction;


    private Vector2 radius;
    private bool charging;

    public override void Fire(Vector2 direction)
    {
        getDir = direction;
        charging = false;
        PlatformerController.instance.StopCoroutine(Power());
        chargeTimer = 0f;
        launchForce = 10 + FireBallSize;
        newFireBall.GetComponent<Rigidbody2D>().gravityScale = 3f;
        newFireBall.GetComponent<Rigidbody2D>().velocity = launchForce * direction;
        newFireBall.GetComponent<Collider2D>().enabled = true;
        FireBallSize = 3f;
    }

    public override void Charge(Vector2 direction)
    {
        chargeTimer = 0f;
        damage = 0;
        newFireBall = Instantiate(fireBall, fireBall.transform.position, Quaternion.identity);
        //newFireBall.transform.position = PlatformerController.instance.transform.position + ((Vector3)direction*1.5f);
        //newFireBall.transform.position *= PlatformerController.instance.coll.size;
        newFireBall.GetComponent<Collider2D>().enabled = false;
        charging = true;
        this.direction = direction;
        PlatformerController.instance.StartCoroutine(bulletUpdate());
        PlatformerController.instance.StartCoroutine(Power());
        
    }
    IEnumerator Power()
    {
        
        while (chargeTimer < 1 && charging == true)
        {
            chargeTimer += Time.fixedDeltaTime;
            FireBallSize = 2 + chargeTimer * 4;
            newFireBall.transform.localScale = new Vector3(FireBallSize, FireBallSize, FireBallSize);
            damage = 2 + chargeTimer * 2;
            yield return null;
        }
        yield return new WaitForSeconds(0f);
    }
    
    public override void OnAimChange(Vector2 direction)
    {
        this.direction = direction;
        newFireBall.transform.position = (Vector2)PlatformerController.instance.transform.position + (direction * 2);
        //newFireBall.transform.position *= PlatformerController.instance.coll.size;
    }
    
    IEnumerator bulletUpdate()
    {
        while (newFireBall.GetComponent<Collider2D>()?.enabled == false)
        {
            newFireBall.transform.position = (Vector2)PlatformerController.instance.transform.position + (direction * 2);
            yield return null;
        }
    }
}
