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

    public virtual void TakeDamage(float baseDamage){
        health -= baseDamage;
    }

    public virtual void OnDeath(){
        //AAAAAA I'm Dying!!! 💀
        GameObject.Destroy(this.gameObject, 0);
    }
}

[CreateAssetMenu(fileName = "EntityData", menuName = "LTG5/Entities/EntityData", order = 0)]
public class EntityData : ScriptableObject{

    public enum elementTypes{None, Earth, Fire, Water, Air};
    public elementTypes element;
    
    public float MaxHealth;
    public float manaDropAmount;

}
