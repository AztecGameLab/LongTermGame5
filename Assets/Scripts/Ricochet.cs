using UnityEngine;

[CreateAssetMenu(fileName = "Ricochet", menuName = "LTG5/Weapons/Ricochet")]
public class Ricochet : ProjectileWeapon
{
    public float speed = 25.0f;
    public float damage = 1;
    public GameObject bullet;
    public float radius;

    private RicochetBullet _chargingBullet;
    
    public override void Fire(Vector2 dir)
    {
        _chargingBullet.rb.velocity = _chargingBullet.transform.right * speed;
        _chargingBullet.coll.enabled = true;
    }

    public override void Charge(Vector2 dir)
    {
        _chargingBullet = Instantiate(bullet).GetComponent<RicochetBullet>();
        _chargingBullet.coll.enabled = false;

        UpdateBulletTransform(dir);
    }

    public override void OnAimChange(Vector2 dir)
    {
        if (_chargingBullet == null)
            return;
        
        UpdateBulletTransform(dir);   
    }

    private void UpdateBulletTransform(Vector2 input)
    {
        var degrees = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        var newPos = PlatformerController.instance.transform.position + (Vector3) input * radius;
        var newRot = Quaternion.Euler(Vector3.forward * degrees);
        
        _chargingBullet.transform.SetPositionAndRotation(newPos, newRot);
    }
    
}
