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
        fireBall.transform.position = PlatformerController.instance.transform.position + ((Vector3)direction*1.5f);
        fireBall.transform.position *= PlatformerController.instance.coll.size;
        newFireBall = Instantiate(fireBall, fireBall.transform.position, Quaternion.identity);
        newFireBall.GetComponent<Collider2D>().enabled = false;
        charging = true;
        PlatformerController.instance.StartCoroutine(Power());
        
    }
    IEnumerator Power()
    {
        
        while (chargeTimer < 3 && charging == true)
        {
            chargeTimer += Time.fixedDeltaTime;
            FireBallSize = 3 + chargeTimer;
            newFireBall.transform.localScale = new Vector3(FireBallSize, FireBallSize, FireBallSize);
            damage += 0.5f;
            yield return null;
        }
        yield return new WaitForSeconds(0f);
    }
    
    public override void OnAimChange(Vector2 direction) {
        newFireBall.transform.position = PlatformerController.instance.transform.position + ((Vector3)direction*1.5f);
        newFireBall.transform.position *= PlatformerController.instance.coll.size;
    }
}
