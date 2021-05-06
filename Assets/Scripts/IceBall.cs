using FMODUnity;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    [SerializeField, EventRef] private string firedSound;
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private Collider2D collider2d;
    
    public void Launch(Vector2 targetVelocity, float gravityForce)
    {
        rigidbody2d.simulated = true;
        rigidbody2d.gravityScale = gravityForce;
        rigidbody2d.velocity = targetVelocity;
        collider2d.enabled = true;
        
        RuntimeManager.PlayOneShot(firedSound);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && collision.rigidbody != null) 
            collision.gameObject.AddComponent<Freeze>();

        Destroy(gameObject);
    }
}