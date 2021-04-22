using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class TriggeredTorch : MonoBehaviour
{
    [EventRef] public string A = "Default";
   [SerializeField]
    private GameObject Light;
 
    [SerializeField]
    private bool lightSwitch;

    public Sprite onTorch;
    public Sprite offTorch;
    void Awake()
    {
        Light.SetActive(lightSwitch);   // level designers can choose the state of the torch, 
                                        // affects if the torch will be on or off when the scene is loaded
        if(lightSwitch == true)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = onTorch;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = offTorch;
        }
      
     

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fireball")      //checking that the game object that collides is the Fireball
        {
            Light.SetActive(true);                       // turning on the torch when the fireball hits it
            if(A != "Default")
            {
                RuntimeManager.PlayOneShot(A);
            }
            this.gameObject.GetComponent<SpriteRenderer>().sprite = onTorch;   //switching the sprite 
        }

    }
}
