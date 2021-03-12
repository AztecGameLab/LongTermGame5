using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCollider : MonoBehaviour
{
    public GameObject fireball;
    
    void Start()
    {
        Entity[] entities = FindObjectsOfType<Entity>();
        Destroy(entities);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(fireball);
    }
}
