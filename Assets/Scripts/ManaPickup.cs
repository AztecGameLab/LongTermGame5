using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPickup : MonoBehaviour
{
    public float mana; //How much mana each orb will have

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TempPlayer")
        {
            ManaSystem.instance.Gain(mana);
            GameObject.Destroy(this.gameObject, 0);
        }
    }
}
