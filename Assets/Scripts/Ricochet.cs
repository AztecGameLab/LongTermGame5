using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RicochetWeapon", menuName = "RicochetWeapon")]
public class Ricochet : ProjectileWeapon
{
    public float speed = 20.0f;
    public float damage = 1;
    public int numReflections = 5;
    public GameObject bullet;
    public RicochetBullet curBullet;
    public Transform firePoint;

    public override void Fire(Vector2 direction)
    {
        GameObject bulletGo = Instantiate(bullet, firePoint.position, Quaternion.identity);
    }

    public override void Charge(Vector2 direction)
    {
        GameObject bulletGo = Instantiate(bullet, firePoint.position, Quaternion.identity);
    }

    public override void OnAimChange(Vector2 direction)
    {
        GameObject bulletGo = Instantiate(bullet, firePoint.position, Quaternion.identity);
    }

}
