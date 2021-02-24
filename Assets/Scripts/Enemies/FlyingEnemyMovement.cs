using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
    PlatformerController player;
    [SerializeField] float speed;
    [SerializeField] float playerRange;

    Vector3 amountToMove;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlatformerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,player.transform.position, speed*Time.deltaTime);
    
    }
}
