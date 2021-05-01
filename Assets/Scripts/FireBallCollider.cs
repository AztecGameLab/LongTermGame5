using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using FMODUnity;

public class FireBallCollider : MonoBehaviour
{
    [SerializeField] private FireBallStats stats;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private StudioEventEmitter fireballSound;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private Collider2D collider2d;

    public void Launch(Vector2 targetVelocity)
    {
        rigidbody2d.gravityScale = 3f;
        rigidbody2d.velocity = targetVelocity;
        collider2d.enabled = true;
        SetFireballAudioParameter(1);
    }

    private void SetFireballAudioParameter(int parameter)
    {
        // FMOD "Fireball" labeled parameter values:
        // 0 = Charge Loop
        // 1 = Throw Loop
        // 2 = Hit Sound
        
        fireballSound.SetParameter("Fireball", parameter);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        DealDirectHitDamage(other.gameObject);
        KnockbackNearbyRigidbodies();
        SetFireballAudioParameter(2);
        
        StartCoroutine(ExplosionAnimation());
    }

    private IEnumerator ExplosionAnimation()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, quaternion.identity);
        sprite.enabled = false;
        rigidbody2d.simulated = false;
        
        yield return new WaitForSecondsRealtime(0.2f);
        Destroy(explosion);
        
        yield return new WaitForSecondsRealtime(3f); // Give the explosion tail time to finish playing
        Destroy(gameObject);
    }

    private void DealDirectHitDamage(GameObject other)
    {
        if(other.TryGetComponent<Entity>(out var entity))
            entity.TakeDamage(stats.damage);
    }

    private void KnockbackNearbyRigidbodies()
    {
        var colliders = Scanner.GetObjectsInRange<Rigidbody2D>(transform.position, stats.radius);

        foreach (var col in colliders)
        {
            Vector2 targetDirection = (col.gameObject.transform.position - transform.position).normalized;
            var speed = col.CompareTag("Player") ? stats.playerLaunchSpeed : stats.normalLaunchSpeed;
            col.velocity = targetDirection * speed;
        }
    }
}
