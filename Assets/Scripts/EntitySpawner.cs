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

        public Enemy(GameObject prefab, Transform position)
        {
            //construct the Entity
            enemy = prefab;
            pos = position;
        }

        //Spawn a single entity
        public void Spawn()
        {
            Instantiate(enemy, pos.position, pos.rotation);
        }

        //sets the enemy pos to a new pos
        public void SetPostition(Transform newPos)
        {
            pos = newPos;
        }
    }

    //available to level designers
    [SerializeField] List<GameObject> prefabs;      //Entities that are in the Spawn Group
    [SerializeField] List<Transform> positions;     //Locations to spawn each Entity
    [SerializeField] Boolean spawnOnStart;          //Spawn Entities when level loads
    [SerializeField] Boolean spawnOnTrigger;         //Spawn Entities when player enters collider trigger
    [SerializeField] Boolean shufflePositions;      //shuffle the positions each time an Entity is spawned
    [SerializeField] float timeBtwnEachSpawn;       //time in between each enemy spawn within each spawnGroup
    [SerializeField] float spawnGroupCooldown;      //time in between each spawnGroup

    float timeSinceLastSpawn;//variable to keep track of when cooldown is up and spawngroup can be spawned again

    //made automatically with prefabs and positions
    List<Enemy> entities;


    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastSpawn = Mathf.Infinity;

        entities = new List<Enemy>();   //instantiate the entities list

        //create the list of Entities matching the prefabs with the corresponding positions
        for(int x = 0; x < prefabs.Count; x++)
            entities.Add(new Enemy(prefabs[x], positions[x]));

        if (spawnOnStart)   //if designer sets this to true 
            StartCoroutine(SpawnGroup());

        
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
    }

    //spawns the entity SpawnGroup
    IEnumerator SpawnGroup()
    {
        if (timeSinceLastSpawn > spawnGroupCooldown)
        {
            timeSinceLastSpawn = 0;
            if (shufflePositions)    //if the deisgner chose to shuffle positions
                Shuffle();
            foreach (Enemy enemy in entities)
            {
                enemy.Spawn();
                yield return new WaitForSeconds(timeBtwnEachSpawn);
            }
               
        }
    }

    //assign each enemy with a new position
    private void Shuffle()
    {
        List<Transform> shuffPos = new List<Transform>();       //new shuffled positions
        System.Random rand = new System.Random();
        int x;

        while (positions.Count > 0)
        {
            //get a randome int range [0, list size)
            x = rand.Next(positions.Count);

            //assign the random selected Transfor as next element in list
            shuffPos.Add(positions[x]);
            positions.RemoveAt(x);      //remove it so it isnt picked again
            
        }

        positions = shuffPos;

        //reassign each enemy with a different position
        for (x = 0; x < entities.Count; x++)
        {
            entities[x].SetPostition(positions[x]);
        }

    }

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlatformerController>())
            if (spawnOnTrigger)    //ensure spawn on trigger is enabled and the cooldown is over
                StartCoroutine(SpawnGroup());
    }

}
