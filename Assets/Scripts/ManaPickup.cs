using FMODUnity;
using UnityEngine;

/**************************************************************************************************
 * Script meant to be added on to the mana orb prefab
 * Orb will use OnCollisionEnter2D in order to detect when the player collides with the mana orb
 * On collision, mana orb will be consumed and a set amount of mana will fill up the Fill Bar
**************************************************************************************************/
public class ManaPickup : MonoBehaviour
{
    [SerializeField] private float mana; //How much mana each orb will have
    [EventRef] public string pickupSound;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) //If an orb collides with whatever the object of the player's name is
        {
            RuntimeManager.PlayOneShot(pickupSound, transform.position);
            UiController.Get().Gain(mana);
            Destroy(gameObject);
        }
    }
}
