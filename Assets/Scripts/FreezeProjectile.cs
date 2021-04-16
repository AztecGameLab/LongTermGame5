using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
[CreateAssetMenu(fileName = "FreezeProjectile", menuName = "LTG5/Weapons/IceBall", order = 0)]
public class FreezeProjectile : ProjectileWeapon
{
    public GameObject iceBall;
   public  GameObject newIceBall; // prefab is set in the Unity
    [SerializeField]
    private float iceBallSpeed; // launchforce
    [SerializeField]
    private float IceBallSize = 3f; 
    [SerializeField]
    private float upForce;
    [SerializeField]
    private float gravityForce;
    private float chargeTime;
    private bool isCharging;
    public Vector2 position;
    public Vector2 radius;
    // called when the player releases the button
    override
    public  void Fire(Vector2 direction)
    {
        position = direction;
        isCharging = false;
        PlatformerController.instance.StopCoroutine(IceBallPower());
        chargeTime = 0f;
        iceBallSpeed = 10 + IceBallSize;
        
        newIceBall.GetComponent<Rigidbody2D>().gravityScale = gravityForce;
        newIceBall.GetComponent<Rigidbody2D>().velocity = iceBallSpeed*direction + (Vector2)(newIceBall.transform.up * upForce);
        newIceBall.GetComponent<Collider2D>().enabled = true;
        
        IceBallSize = 3f;
    }
    //called when the player hold down the button
    override
    public void Charge(Vector2 direction)
    {
        chargeTime = 0f;
        iceBall.transform.position = PlatformerController.instance.transform.position + ((Vector3)direction * 1.5f);//
        iceBall.transform.position *= PlatformerController.instance.coll.size;
        newIceBall = Instantiate(iceBall, iceBall.transform.position, Quaternion.identity);
        newIceBall.GetComponent<Collider2D>().enabled = false;
        isCharging = true;
        PlatformerController.instance.StartCoroutine(IceBallPower());
    }
    //runs after charge
    override
    public void OnAimChange(Vector2 direction)
    {
        newIceBall.transform.position = PlatformerController.instance.transform.position + ((Vector3)direction * 1.5f);
        newIceBall.transform.position *= PlatformerController.instance.coll.size;
    }
   private IEnumerator IceBallPower()
    {
        while (chargeTime < 3 && isCharging == true)
        {
            chargeTime += Time.fixedDeltaTime;
            IceBallSize = 3 + chargeTime;
            newIceBall.transform.localScale = new Vector3(IceBallSize, IceBallSize, IceBallSize);
            yield return null;
        }
        yield return new WaitForSeconds(0f);
    }

}
