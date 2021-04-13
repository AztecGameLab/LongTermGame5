using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Ricochet", menuName = "LTG5/Weapons/Ricochet")]
public class Ricochet : ProjectileWeapon
{
    [SerializeField] private float degreesPerSecond = 360f;
    public float speed = 25.0f;
    public float damage = 1;
    public GameObject bullet;
    public float radius;

    private RicochetBullet _chargingBullet;
    private Coroutine _chargingAnimation;
    private float _targetDegrees = 0f;
    private Vector2 _lastInput;
    private float CurrentDegrees => _chargingBullet.transform.rotation.eulerAngles.z;
    
    public override void Fire(Vector2 dir)
    {
        if (_chargingAnimation != null)
            PlatformerController.instance.StopCoroutine(_chargingAnimation);
        
        _chargingBullet.rb.velocity = _chargingBullet.transform.right * speed;
        _chargingBullet.coll.enabled = true;
    }

    public override void Charge(Vector2 dir)
    {
        var spawnPos = dir == Vector2.zero ? (Vector2) PlatformerController.instance.transform.right : dir;
        
        _chargingBullet = Instantiate(bullet).GetComponent<RicochetBullet>();
        _chargingBullet.coll.enabled = false;
        _lastInput = dir;
        
        UpdateBulletTransform(spawnPos);
        OnAimChange(spawnPos);
    }

    public override void OnAimChange(Vector2 dir)
    {
        if (_chargingBullet == null || dir == Vector2.zero)
            return;

        var player = PlatformerController.instance;
        _targetDegrees = PositionToDegrees(dir);
        _lastInput = dir;
        
        if (_chargingAnimation != null)
            player.StopCoroutine(_chargingAnimation);
        
        _chargingAnimation = player.StartCoroutine(MoveToTargetPosition());
    }

    private IEnumerator MoveToTargetPosition()
    {
        var targetA = _targetDegrees;
        var targetB = CurrentDegrees > targetA ? targetA + 360: targetA - 360;
        var targetADistance = Mathf.Abs(CurrentDegrees - targetA);
        var targetBDistance = Mathf.Abs(CurrentDegrees - targetB);
        var closestTarget = targetADistance < targetBDistance ? targetA : targetB;

        var origin = PlatformerController.instance.transform.position;
        var axis = Vector3.forward;
        var degreeDelta = degreesPerSecond * Time.unscaledDeltaTime;
        var direction = Mathf.Sign(closestTarget - CurrentDegrees);

        var realTarget = (closestTarget < 0 ? 360 + closestTarget : closestTarget) % 360;
        
        while (Mathf.Abs(realTarget - CurrentDegrees) >= degreeDelta)
        {
            origin = PlatformerController.instance.transform.position;
            _chargingBullet.transform.RotateAround(origin, axis, direction * degreeDelta);
            yield return null;
        }
        
        while (true)
        {
            UpdateBulletTransform(_lastInput);
            yield return null;
        }
    }

    private static float PositionToDegrees(Vector2 position)
    {
        return Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
    }

    private void UpdateBulletTransform(Vector2 position)
    {
        var degrees = PositionToDegrees(position);
        var newPos = PlatformerController.instance.transform.position + (Vector3) position * radius;
        var newRot = Quaternion.Euler(Vector3.forward * degrees);

        _chargingBullet.transform.SetPositionAndRotation(newPos, newRot);
    }
    
}
