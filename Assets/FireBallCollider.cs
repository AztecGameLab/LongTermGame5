using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCollider : MonoBehaviour
{
    public GameObject fireball;
    public GameObject player;
    private Vector3 spawn;
    
    void Start()
    {
        player = GameObject.Find("TempPlayer");
        fireball.transform.position = player.transform.position;
        spawn = fireball.transform.position;
        spawn.x += 1;
        fireball.transform.position = spawn;
        
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(fireball);
    }
}
