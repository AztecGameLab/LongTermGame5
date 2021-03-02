using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField]public class Entity
    {
        public GameObject Object;
        public Transform pos;
    }
  
    [SerializeField]List<Entity> Entities;      //Entities that are in the Spawn Group
    [SerializeField] Boolean SpawnOnStart;          //Spawn Entities when level loads
    [SerializeField] Boolean SpawnOnTrigger;        //Spawn Entities on trigger
    [SerializeField] Collider2D SpawnCollide;       //Collider used as trigger to spawn Entities


    // Start is called before the first frame update
    void Start()
    {

    }

 
    // Update is called once per frame
    void Update()
    {
        
    }
}
