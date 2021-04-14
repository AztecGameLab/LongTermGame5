using UnityEngine;

public class RicochetBullet : MonoBehaviour
{
    private float damage = 1;
    public float knockback = 0.5f;
    private int reflectionsRemaining = 5;
    public Rigidbody2D rb;
    public Collider2D coll;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Entity entity = other.gameObject.GetComponent<Entity>();
        
        if (entity != null)
        {
            entity.TakeDamage(damage, rb.velocity.normalized * knockback);
            Destroy(gameObject);
        }
        else
        {
            if (reflectionsRemaining > 0)
            {
                --reflectionsRemaining;
            }
            else Destroy(gameObject);
        }
    }
}
