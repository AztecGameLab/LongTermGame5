using System.Collections;
using FMODUnity;
using UnityEngine;

public class FlyingEnemyProjectile : MonoBehaviour
{
    [SerializeField] float speed =10f;
    [SerializeField] float Damage = 2f;
    [SerializeField, EventRef] private string launchSound = "event:/Enemies/Sky Enemy/Sky Enemy Projectile";
    Rigidbody2D rb;
    PlatformerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlatformerController.instance;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce((player.transform.position - transform.position).normalized * speed);

        //destroy itself after 5 seconds
        StartCoroutine(DestroyProjectile());
        RuntimeManager.PlayOneShotAttached(launchSound, gameObject);
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<PlatformerController>();
        if (player)
        {
            player.TakeDamage(Damage);
        }
        
        Destroy(gameObject);
    }
}
