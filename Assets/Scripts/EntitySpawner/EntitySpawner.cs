using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField]List<GameObject> Entities;      //Entities that are in the Spawn Group
    [SerializeField] Boolean SpawnOnStart;          //Spawn Entities when level loads
    [SerializeField] Boolean SpawnOnTrigger;        //Spawn Entities on trigger
    [SerializeField] Collider2D SpawnCollide;       //Collider used as trigger to spawn Entities


    // Start is called before the first frame update
    void Start()
    {
       
    }

    //Spawns an entity
    void Spawn(GameObject Entity)
    {
        //spawns given entity at given transform(position)
        Instantiate(Entity, Entity.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
