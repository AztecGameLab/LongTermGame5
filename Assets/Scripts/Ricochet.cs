using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "Ricochet", menuName = "LTG5/Weapons/Ricochet")]
public class Ricochet : ProjectileWeapon
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float degreesPerSecond = 360f;
    [SerializeField] private float speed = 25.0f;
    [SerializeField] private float radius;
    [SerializeField] private float spawnAnimationTime = 0.25f;
    [SerializeField,EventRef] private string ricochetSoundLaunch;
    
    private Transform _bulletHolder;
    private RicochetBullet _chargingBullet;
    private PlatformerController _player;
    private Coroutine _movementAnimation;
    private Coroutine _spawnAnimation;
    private float _targetDegrees = 0f;
    private float CurrentDegrees => _chargingBullet.transform.rotation.eulerAngles.z;

    public override void Charge(Vector2 dir)
    {
        var spawnPos = dir == Vector2.zero ? (Vector2) PlatformerController.instance.transform.right : dir;
        
        _player = PlatformerController.instance;
        SpawnBullet();
        _spawnAnimation = _player.StartCoroutine(SpawnAnimation());
        UpdateBulletTransform(spawnPos);
        OnAimChange(spawnPos);
    }
    
    public override void OnAimChange(Vector2 dir)
    {
        if (_chargingBullet == null || dir == Vector2.zero)
            return;

        _targetDegrees = PositionToDegrees(dir);
        
        if (_movementAnimation != null)
            _player.StopCoroutine(_movementAnimation);
        
        _movementAnimation = _player.StartCoroutine(MoveToTargetPosition());
    }
    
    public override void Fire(Vector2 dir)
    {
        if (_chargingBullet == null)
            return;
    
        if (_movementAnimation != null)
            _player.StopCoroutine(_movementAnimation);
        
        _chargingBullet.transform.SetParent(_bulletHolder);
        _chargingBullet.rb.bodyType = RigidbodyType2D.Dynamic;
        _chargingBullet.rb.velocity = _chargingBullet.transform.right * speed;
        _chargingBullet.coll.enabled = true;
        RuntimeManager.PlayOneShot(ricochetSoundLaunch);
    }

    public override void Cancel()
    {
        if (_player.lockControls)
            DestroyActiveBullet();
    }
    
    private void SpawnBullet()
    {
        if (_bulletHolder == null)
            _bulletHolder = new GameObject("Fired Ricochet Projectiles").transform;
        
        _chargingBullet = Instantiate(bullet).GetComponent<RicochetBullet>();
        _chargingBullet.coll.enabled = false;
        _chargingBullet.rb.bodyType = RigidbodyType2D.Kinematic;
        _chargingBullet.transform.SetParent(_player.transform);
    }

    private void DestroyActiveBullet()
    {
        if (_movementAnimation != null)
            _player.StopCoroutine(_movementAnimation);
        
        if (_spawnAnimation != null)
            _player.StopCoroutine(_spawnAnimation);
        
        if (_chargingBullet != null)
            Destroy(_chargingBullet.gameObject);
    }

    private IEnumerator SpawnAnimation()
    {
        var elapsedTime = 0f;
        var chargingBullet = _chargingBullet; // Cache the charging bullet so when it changes, animation still works
        
        while (elapsedTime < spawnAnimationTime && chargingBullet != null)
        {
            var completion = elapsedTime / spawnAnimationTime;
            chargingBullet.transform.localScale = Vector3.Slerp(Vector3.zero, Vector3.one, completion);
            elapsedTime += Time.unscaledDeltaTime; // Unscaled because of TimeScale changing during animation
            yield return null;
        }
    }

    private IEnumerator MoveToTargetPosition()
    {
        var targetA = _targetDegrees;
        var targetB = CurrentDegrees > targetA ? targetA + 360: targetA - 360;
        var targetADistance = Mathf.Abs(CurrentDegrees - targetA);
        var targetBDistance = Mathf.Abs(CurrentDegrees - targetB);
        
        var closestTarget = targetADistance < targetBDistance ? targetA : targetB;
        var closestTargetWrapped = (closestTarget < 0 ? 360 + closestTarget : closestTarget) % 360;
        
        var axis = Vector3.forward;
        var degreeDelta = degreesPerSecond * Time.unscaledDeltaTime;
        var direction = Mathf.Sign(closestTarget - CurrentDegrees);
        
        // Execute while the distance needed is greater than our movement speed
        while (Mathf.Abs(closestTargetWrapped - CurrentDegrees) >= degreeDelta)
        {
            var origin = _player.transform.position;
            _chargingBullet.transform.RotateAround(origin, axis, direction * degreeDelta);
            yield return new WaitForEndOfFrame();
        }
    }

    private static float PositionToDegrees(Vector2 position)
    {
        return Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
    }

    private void UpdateBulletTransform(Vector2 position)
    {
        var degrees = PositionToDegrees(position);
        var newPos = _player.transform.position + (Vector3) position * radius;
        var newRot = Quaternion.Euler(Vector3.forward * degrees);

        _chargingBullet.transform.SetPositionAndRotation(newPos, newRot);
    }
}