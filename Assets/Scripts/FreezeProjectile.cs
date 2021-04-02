using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FreezeProjectile : MonoBehaviour
{
    public GameObject iceBall;
    GameObject newIceBall;
    public Transform shootingPosition;
    [SerializeField]
    private float iceBallSpeed;
    [SerializeField]
    public float chargeTimer;
    [SerializeField]
    private float IceBallSize = 5f;

    public void Fire()
    {
        newIceBall = Instantiate(iceBall, shootingPosition.position, shootingPosition.rotation); //creates an iceball
        IceBallSize += chargeTimer; //the longer the iceball is charged the bigger it will get
        newIceBall.transform.localScale = new Vector3(IceBallSize, IceBallSize, IceBallSize); //
        newIceBall.GetComponent<Rigidbody2D>().velocity = (newIceBall.transform.right * iceBallSpeed);
        chargeTimer = 0f; // resets the timer
        IceBallSize = 5f; // resets the size of the iceball
    }
    public void Charge()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        
    }
}
