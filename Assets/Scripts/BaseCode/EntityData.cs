using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "LTG5/Entities/EntityData", order = 0)]
public class EntityData : ScriptableObject{

    public enum elementTypes{None, Earth, Fire, Water, Air};
    public elementTypes element;
    
    public float MaxHealth;
    public float manaDropAmount;

}