using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredTorch : MonoBehaviour
{
 
   [SerializeField]
    private GameObject Torch;
    private Collider2D fireBall;
    private Collider2D onTorchCol;
    public Sprite onTorch; 

     void Start()
    {
        fireBall = GameObject.Find("Fireball").GetComponent<Collider2D>();
        onTorchCol = GameObject.Find("Torch").GetComponent<Collider2D>();
    }
    void Awake()
    {
        Torch.SetActive(false);   // ensuring the torch is off by default
    }
    void OnCollisionEnter2D(Collision2D collision)
    {     
        if (collision.gameObject.tag == "Fireball")      //checking that the game object that collides is the Fireball
        {
            Torch.SetActive(true);
            Physics2D.IgnoreCollision(onTorchCol, fireBall, true); //turning off collision in order for the fireball to travel through the torch
            this.gameObject.GetComponent<SpriteRenderer>().sprite = onTorch;
        }
    }
}
