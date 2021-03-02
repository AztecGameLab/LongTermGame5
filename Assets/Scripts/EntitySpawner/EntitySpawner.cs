using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    //subclass to store data about the entity
    public class Entity
    {
        private GameObject enemy;        //variable to store the Enemy type
        private Transform pos;           //varibale to store the spawn position
        private int spawnCount;          //store how many times this Entity has been spawned

        public Entity(GameObject prefab, Transform position)
        {
            //construct the Entity
            enemy = prefab;
            pos = position;
            spawnCount = 0;      //intialize amount of spawns to 0
        }

        //Spawns a single entity
        public void Spawn()
        { 
            Instantiate(enemy, pos);
            spawnCount += 1;
        }

        //Spawn a single Entity given a new position, overides the intial one temporarily
        public void Spawn(Transform tempPos)
        {
            Instantiate(enemy, tempPos);
            spawnCount += 1;
        }

    }

    //available to level designers
    [SerializeField] List<GameObject> prefabs;      //Entities that are in the Spawn Group
    [SerializeField] List<Transform> positions;     //Locations to spawn each Entity
    [SerializeField] Boolean spawnOnStart;          //Spawn Entities when level loads
    [SerializeField] Boolean spawnOnTrigger;        //Spawn Entities on trigger
    [SerializeField] Collider2D spawnCollider;      //Collider used as trigger to spawn Entities
    [SerializeField] Boolean shufflePositions;      //shuffle the positions each time an Entity is spawned

    //made automatically with information from above
    List<Entity> entities;


    // Start is called before the first frame update
    void Start()
    {
        //create the list of Entities matching the prefabs with the corresponding positions
        for(int x = 0; x < prefabs.Count - 1; x++)
        {
            entities.Add(new Entity(prefabs[x], positions[x]));
        }

        if (spawnOnStart)
            SpawnGroup();
    }

    //spawns the entity SpawnGroup
    void SpawnGroup()
    {
        if (shufflePositions)
        {
            positions = Shuffle(positions);

            //traverse through the list and spawn each entity with a shuffled position
            for (int x = 0; x < entities.Count; x++)
            {
                entities[x].Spawn(positions[x]);
            }
        }
        else
            foreach (Entity enemy in entities)
                enemy.Spawn();
    }

    private List<Transform> Shuffle(List<Transform> orderedPos)
    {
        List<Transform> shuffPos = new List<Transform>();       //new shuffled positions
        System.Random rand = new System.Random();
        int x;

        while (orderedPos.Count > 0)
        {
            //get a randome int range [0, list size)
            x = rand.Next(orderedPos.Count);

            //assign the random selected Transfor as next element in list
            shuffPos.Add(orderedPos[x]);
            orderedPos.RemoveAt(x);      //remove it so it isnt picked again
            
        }

        return shuffPos;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void OnValidate()
    {
        foreach(GameObject Entity in entities)
        {
            Spawn(Entity, Entity.transform);
        }
    }
    */
}
