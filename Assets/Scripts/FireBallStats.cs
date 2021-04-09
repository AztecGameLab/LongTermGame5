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
    private bool rocket;
    public int damage;

    private float FireBallSize = 3f;   

    public override void Fire(Vector2 direction)
    {
        PlatformerController.instance.StopCoroutine(Power());
        chargeTimer = 0f;
        Destroy(newFireBall);
        FireBallSize = 3f;
    }

    public override void Charge(Vector2 direction)
    {
        chargeTimer = 0f;
        damage = 0;
        fireBall.transform.position = PlatformerController.instance.transform.position + (Vector3)direction;
        fireBall.transform.position *= PlatformerController.instance.coll.size;
        newFireBall = Instantiate(fireBall, fireBall.transform.position, fireBall.transform.rotation);
        newFireBall.GetComponent<Collider2D>().enabled = false;
        PlatformerController.instance.StartCoroutine(Power());
    }
    IEnumerator Power()
    {
        Debug.Log("PRINTPRINT");
        while (chargeTimer < 3)
        {
            chargeTimer += Time.deltaTime;
            FireBallSize += chargeTimer;
            newFireBall.transform.localScale = new Vector3(FireBallSize, FireBallSize, FireBallSize);
            damage += 1;
            yield return null;
        }
        
    }
    
    public override void OnAimChange(Vector2 direction) {
        newFireBall.transform.position = PlatformerController.instance.transform.position + (Vector3)direction;
        newFireBall.transform.position *= PlatformerController.instance.coll.size;
    }
}
