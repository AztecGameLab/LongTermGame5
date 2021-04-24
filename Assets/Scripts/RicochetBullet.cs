using FMODUnity;
using UnityEngine;

public class RicochetBullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damage = 1;
    [SerializeField] private float knockback = 0.5f;
    [SerializeField] private int reflectionsRemaining = 5;
    
    [Header("References")]
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private Collider2D collider2d;
    [SerializeField, EventRef] private string bounceSound;
    [SerializeField, EventRef] private string hitSound;

    public void Fire(float speed)
    {
        rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2d.velocity = transform.right * speed;
        collider2d.enabled = true;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Entity entity = other.gameObject.GetComponent<Entity>();
        
        if (entity != null)
        {
            entity.TakeDamage(damage, rigidbody2d.velocity.normalized * knockback);
            RuntimeManager.PlayOneShot(hitSound, transform.position);
            Destroy(gameObject);
        }
        else
        {
            RuntimeManager.PlayOneShot(bounceSound, transform.position);
            
            if (reflectionsRemaining > 0)
                --reflectionsRemaining;
            else 
                Destroy(gameObject);
        }
    }
}
