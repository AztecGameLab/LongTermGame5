using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    
    public Transform FirePoint;
    public GameObject fireBall;
    
    public float launchForce;

    private float fireBallSize = 3f;
    public float chargeTimer;
    public KeyCode charge;
    
   
    void Update()
    {
        if (Input.GetKey(charge))
        {
            if(chargeTimer < 2)
            {
                chargeTimer += Time.deltaTime;
                Debug.Log(chargeTimer);
            }
        }
        if (Input.GetKeyUp(charge))
        {
            fireBallSize = fireBallSize * 2f * chargeTimer;
            Shoot();
        }
    }

    void ChargeFire()
    {
        
    }
    void Shoot()
    {
        
        GameObject newFireBall = Instantiate(fireBall, FirePoint.position, FirePoint.rotation);
        newFireBall.transform.localScale = new Vector3(fireBallSize, fireBallSize, fireBallSize);
        newFireBall.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        fireBallSize = 3f;
        chargeTimer = 0f;
    }
}
