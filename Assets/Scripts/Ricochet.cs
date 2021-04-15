using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ricochet", menuName = "LTG5/Weapons/Ricochet")]
public class Ricochet : ProjectileWeapon
{
    public float speed = 25.0f;
    public GameObject bullet;
    private RicochetBullet currBullet;
    private Vector2 direction;
    public float radius;
    [EventRef] public string ricochetSoundLaunch = "Default";

    public override void Fire(Vector2 direction)
    {
        currBullet.gameObject.transform.position = PlatformerController.instance.transform.position + (Vector3) direction * radius;
        currBullet.rb.velocity = direction * speed;
        currBullet.coll.enabled = true;
        if (ricochetSoundLaunch != "Default")
        {
            RuntimeManager.PlayOneShot(ricochetSoundLaunch);
        }
    }

    public override void Charge(Vector2 direction)
    {
        GameObject bulletGo = Instantiate(bullet, PlatformerController.instance.transform.position + (Vector3) direction * radius, Quaternion.identity);
        currBullet = bulletGo.GetComponent<RicochetBullet>();
        currBullet.coll.enabled = false;
        this.direction = direction;
        PlatformerController.instance.StartCoroutine(bulletUpdate());
    }

    public override void OnAimChange(Vector2 direction)
    {
        if (currBullet == null)
        {
            return;
        }
        this.direction = direction;
        currBullet.gameObject.transform.position = PlatformerController.instance.transform.position + (Vector3) direction * radius;
    }

    IEnumerator bulletUpdate()
    {
        while (currBullet.coll.enabled == false)
        {
            currBullet.gameObject.transform.position = PlatformerController.instance.transform.position + (Vector3) direction * radius;
            yield return null;
        }
    }

}
