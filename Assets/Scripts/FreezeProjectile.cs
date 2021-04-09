using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
[CreateAssetMenu(fileName = "FreezeProjectile", menuName = "LTG5/Weapons/IceBall", order = 0)]
public class FreezeProjectile : ProjectileWeapon
{
    [EventRef] public string A = "Default";
    public GameObject iceBall;
    GameObject newIceBall; // prefab is set in the Unity
    [SerializeField]
    private float iceBallSpeed;
    [SerializeField]
    private float IceBallSize; //radius
    [SerializeField]
    private float upForce;

    private float chargeTime;
    public Vector2 position = PlatformerController.instance.transform.position;

    // called when the player releases the button
    override
    public void Fire(Vector2 direction)
    {
        chargeTime = chargeTime - Time.deltaTime;
        IceBallSize += chargeTime;
        newIceBall.GetComponent<Rigidbody2D>().velocity = (position + direction * IceBallSize * iceBallSpeed) + (Vector2)(newIceBall.transform.up * upForce);
        if (A != "Default")
        {
            RuntimeManager.PlayOneShot(A);
        }
    }

    //called when the player hold down the button
    override
    public void Charge(Vector2 direction)
    {
        chargeTime += Time.deltaTime;
        newIceBall = Instantiate(iceBall, position + direction * IceBallSize, Quaternion.identity); 
    }
    //runs after charge
    override
    public void OnAimChange(Vector2 direction)
    {
        newIceBall.transform.position = position+direction;
    }
}
