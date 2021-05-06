using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPoundDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        var entity = other.gameObject.GetComponent<Entity>();
        if (entity)
        {
            entity.TakeDamage(3);

            if (entity is GroundPoundDestructable)
            {
                ((GroundPoundDestructable)entity).GroundPoundTakeDamage(3);
            }
        }
        
        Destroy(this);
    }
}
