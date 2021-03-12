using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredTorch : MonoBehaviour
{
 
   [SerializeField]
    private GameObject Light;
    public Sprite onTorch;
    [SerializeField]
    private bool lightSwitch;
    void Awake()
    {
        Light.SetActive(lightSwitch);   // level designers can choose the state of the torch
      
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fireball")      //checking that the game object that collides is the Fireball
        {
            Light.SetActive(true); // turning on the torch when the fireball hits it
            this.gameObject.GetComponent<SpriteRenderer>().sprite = onTorch;   //switching the sprite 
        }

    }
}
