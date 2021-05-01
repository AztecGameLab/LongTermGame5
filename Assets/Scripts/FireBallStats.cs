using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "FireBall", menuName = "FireBall")]
public class FireBallStats : ProjectileWeapon
{
    public FireBallCollider fireBallPrefab;
    private FireBallCollider _newFireBall;
    public float launchForce;
    public float recoil;
    public float upForce;
    public float chargeTimer;
    public float damage;
    public float FireBallSize = 3f;
    public Vector2 getDir;
    private Vector2 direction;
    public float radius = 3f;
    private bool charging;

    public override void Fire(Vector2 direction)
    {
        getDir = direction;
        charging = false;
        PlatformerController.instance.StopCoroutine(Power());
        chargeTimer = 0f;
        launchForce = 10 + FireBallSize;
        FireBallSize = 3f;

        _newFireBall.Launch(launchForce * direction);
    }

    public override void Charge(Vector2 direction)
    {
        chargeTimer = 0f;
        damage = 0;
        _newFireBall = Instantiate(fireBallPrefab, fireBallPrefab.transform.position, Quaternion.identity);
        _newFireBall.GetComponent<Collider2D>().enabled = false;
        charging = true;
        this.direction = direction;
        PlatformerController.instance.StartCoroutine(BulletUpdate());
        PlatformerController.instance.StartCoroutine(Power());
    }
    
    private IEnumerator Power()
    {
        while (chargeTimer < 1 && charging == true)
        {
            chargeTimer += Time.fixedDeltaTime;
            FireBallSize = 2 + chargeTimer * 4;
            _newFireBall.transform.localScale = new Vector3(FireBallSize, FireBallSize, FireBallSize);
            damage = 2 + chargeTimer * 2;
            yield return null;
        }
        yield return new WaitForSeconds(0f);
    }
    
    public override void OnAimChange(Vector2 direction)
    {
        this.direction = direction;
        _newFireBall.transform.position = (Vector2)PlatformerController.instance.transform.position + (direction * 2);
    }
    
    private IEnumerator BulletUpdate()
    {
        while (_newFireBall.GetComponent<Collider2D>()?.enabled == false)
        {
            _newFireBall.transform.position = (Vector2)PlatformerController.instance.transform.position + (direction * 2);
            yield return null;
        }
    }
}
