using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FreezeProjectile : ProjectileWeapon
{
    public GameObject iceBall;
    GameObject newIceBall;
    [SerializeField]
    private float iceBallSpeed;
    [SerializeField]
    private float IceBallSize;
    [SerializeField]
    private float upForce;

    public Vector2 position = PlatformerController.instance.transform.position;

    // called when the player releases the button
    override
    public void Fire(Vector2 direction)
    {
        newIceBall.GetComponent<Rigidbody2D>().velocity = (position + direction * IceBallSize) + (Vector2)(newIceBall.transform.up * upForce);
    }


    //called when the player hold down the button
    override
    public void Charge(Vector2 direction)
    {
        newIceBall = Instantiate(iceBall, position + direction * IceBallSize, Quaternion.identity); 

    }

    //runs after charge
    override
    public void OnAimChange(Vector2 direction)
    {
        newIceBall.transform.position = position+direction;
    }
}
