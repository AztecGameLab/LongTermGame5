using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> Entities;     //Entities that are in the Spawn Group
    [SerializeField] List<Transform> Positions;     //Locations to spawn each Entity
    [SerializeField] Boolean SpawnOnStart;          //Spawn Entities when level loads
    [SerializeField] Boolean SpawnOnTrigger;        //Spawn Entities on trigger
    [SerializeField] Collider2D SpawnCollider;      //Collider used as trigger to spawn Entities
    


    // Start is called before the first frame update
    void Start()
    {
          
    }

    //Spawns an entity
    void Spawn(GameObject Entity, Transform pos)
    {
        //spawns given entity at given transform(position)
        Instantiate(Entity, pos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        foreach(GameObject Entity in Entities)
        {
            Spawn(Entity, Entity.transform);
        }
    }
}
