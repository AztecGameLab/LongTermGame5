using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**************************************************************************************************
 * Script meant to be added on to the mana orb prefab
 * Orb will use OnCollisionEnter2D in order to detect when the player collides with the mana orb
 * On collision, mana orb will be consumed and a set amount of mana will fill up the Fill Bar
**************************************************************************************************/
public class ManaPickup : MonoBehaviour
{
    public float mana; //How much mana each orb will have

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TempPlayer") //If an orb collides with whatever the object of the player's name is
        {
            ManaSystem.instance.Gain(mana); //Calls Gain from FillSystem
            GameObject.Destroy(this.gameObject, 0);
        }
    }
}
