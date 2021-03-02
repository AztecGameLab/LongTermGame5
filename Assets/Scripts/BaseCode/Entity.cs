using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is just a simple base for any damagable objects
/// </summary>
public class Entity : MonoBehaviour
{
    [SerializeField]
    public float health;

    [EasyButtons.Button]
    public virtual void TakeDamage(float baseDamage){
        health -= baseDamage;
        if(health <= 0){
            OnDeath();
        }
    }

    
    public virtual void OnDeath(){
        //AAAAAA I'm Dying!!! 💀
        GameObject.Destroy(this.gameObject, 0);
    }
}
