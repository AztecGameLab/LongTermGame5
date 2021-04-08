using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FreezeProjectile : ProjectileWeapon
{
    public GameObject iceBall;
    GameObject newIceBall;
    public Transform shootingPosition;

    [SerializeField]
    private float iceBallSpeed;
    [SerializeField]
    public float chargeTimer;
    [SerializeField]
    private float IceBallSize;
    [SerializeField]
    private float upForce;
    private float initialIceBallSize;
    override
    public void Fire()
    {
        newIceBall = Instantiate(iceBall, shootingPosition.position, shootingPosition.rotation); //creates an iceball
        initialIceBallSize = IceBallSize;
        IceBallSize += chargeTimer; //the longer the iceball is charged the bigger it will get
        newIceBall.transform.localScale = new Vector3(IceBallSize, IceBallSize, IceBallSize); //
        newIceBall.GetComponent<Rigidbody2D>().velocity = (newIceBall.transform.right * iceBallSpeed) + (newIceBall.transform.up * upForce);
        chargeTimer = 0f; // resets the timer
        IceBallSize = initialIceBallSize; // resets the size of the iceball
    }
    override
    public void Charge()
    {
        if (chargeTimer < 2)
        {
            chargeTimer += Time.deltaTime;

        }
    }

    private void Update()
    {
      //  if (Input.GetMouseButtonDown(0))
      //  {
       //     Fire();
      //  }
        
    }
}
