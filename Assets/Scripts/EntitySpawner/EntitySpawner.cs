using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    //subclass to store data about the enemy
    public class Enemy
    {
        private GameObject enemy;        //variable to store the Enemy type
        private Transform pos;           //varibale to store the spawn position
        private int spawnCount;          //store how many times this Entity has been spawned

        public Enemy(GameObject prefab, Transform position)
        {
            //construct the Entity
            enemy = prefab;
            pos = position;
            spawnCount = 0;      //intialize amount of spawns to 0
        }

        //Spawn a single entity
        public void Spawn()
        {
            Instantiate(enemy, pos.position, pos.rotation);
            spawnCount += 1;
        }

        //Spawn a single entity given a new position, overides the intial one temporarily
        public void Spawn(Transform tempPos)
        {
            Instantiate(enemy, tempPos.position, tempPos.rotation);
            spawnCount += 1;
        }

    }

    //available to level designers
    [SerializeField] List<GameObject> prefabs;      //Entities that are in the Spawn Group
    [SerializeField] List<Transform> positions;     //Locations to spawn each Entity
    [SerializeField] Boolean spawnOnStart;          //Spawn Entities when level loads
    [SerializeField] Boolean spawnOnTrigger;         //Spawn Entities when player enters collider trigger
    [SerializeField] Boolean shufflePositions;      //shuffle the positions each time an Entity is spawned
    [SerializeField] float timeBtwnEachSpawn;       //time in between each enemy spawn within each spawnGroup
    [SerializeField] int spawnGroupCooldown;      //time in between each spawnGroup


    Collider2D colliderTrigger;

    //made automatically with prefabs and positions
    List<Enemy> entities;


    // Start is called before the first frame update
    void Start()
    {
        
        entities = new List<Enemy>();   //instantiate the entities list

        //create the list of Entities matching the prefabs with the corresponding positions
        for(int x = 0; x < prefabs.Count; x++)
        {
            entities.Add(new Enemy(prefabs[x], positions[x]));
        }

        if (spawnOnStart)   //if designer sets this to true
            StartCoroutine(SpawnGroup());
    }

    //spawns the entity SpawnGroup
    IEnumerator SpawnGroup()
    {
        if (shufflePositions)    //if the deisgner chose to shuffle positions
        {
            positions = Shuffle(positions);

            //traverse through the list and spawn each entity with a shuffled position
            for (int x = 0; x < entities.Count; x++)
            {
                yield return new WaitForSeconds(timeBtwnEachSpawn);
                entities[x].Spawn(positions[x]);
            }
        }
        else
            foreach (Enemy enemy in entities)
            {
                enemy.Spawn();
                yield return new WaitForSeconds(timeBtwnEachSpawn);
            }
                
    }

    //shuffles the list of posi
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

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlatformerController>())
            if (spawnOnTrigger)
                StartCoroutine(SpawnGroup());
    }




    // Update is called once per frame
    void Update()
    {
        
    }

}
