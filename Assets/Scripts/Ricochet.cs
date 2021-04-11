using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Ricochet", menuName = "LTG5/Weapons/Ricochet")]
public class Ricochet : ProjectileWeapon
{
    public float speed = 25.0f;
    public float damage = 1;
    public GameObject bullet;
    private RicochetBullet currBullet;
    private Vector2 direction;
    public float radius;

    public override void Fire(Vector2 dir)
    {
        currBullet.gameObject.transform.position = PlatformerController.instance.transform.position + (Vector3) dir * radius;
        currBullet.rb.velocity = dir * speed;
        currBullet.coll.enabled = true;
    }

    public override void Charge(Vector2 dir)
    {
        GameObject bulletGo = Instantiate(bullet, PlatformerController.instance.transform.position + (Vector3) dir * radius, Quaternion.identity);
        currBullet = bulletGo.GetComponent<RicochetBullet>();
        currBullet.coll.enabled = false;
        direction = dir;
        PlatformerController.instance.StartCoroutine(BulletUpdate());
    }

    public override void OnAimChange(Vector2 dir)
    {
        if (currBullet == null)
            return;
        
        direction = dir;
        currBullet.gameObject.transform.position = PlatformerController.instance.transform.position + (Vector3) dir * radius;
        
    }

    private IEnumerator BulletUpdate()
    {
        while (currBullet.coll.enabled == false)
        {
            currBullet.gameObject.transform.position = PlatformerController.instance.transform.position + (Vector3) direction * radius;
            yield return null;
        }
    }

}
