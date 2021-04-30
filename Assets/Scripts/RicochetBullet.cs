using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using FMOD;
using FMOD.Studio;
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
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private StudioEventEmitter spawnAndHitEmitter;
    [SerializeField, EventRef] private string bounceSound;

    private EventInstance _spawnAndHitInstance;
    
    public void Fire(float speed)
    {
        spawnAndHitEmitter.Play();
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
            spawnAndHitEmitter.SetParameter("Air Ricochet", 1);
            Destroy(gameObject);
        }
        else
        {
            RuntimeManager.PlayOneShot(bounceSound, transform.position);

            if (reflectionsRemaining > 0)
                --reflectionsRemaining;
            else
                StartCoroutine(CustomDestroy());
        }
    }

    private IEnumerator CustomDestroy()
    {
        spawnAndHitEmitter.SetParameter("Air Ricochet", 1);
        sprite.enabled = false;
        rigidbody2d.simulated = false;
        
        yield return new WaitForSecondsRealtime(2);
        
        Destroy(gameObject);
    }
}
