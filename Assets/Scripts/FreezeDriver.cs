using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeDriver : MonoBehaviour
{
    [SerializeField] FreezeProjectile IceballScript;
    public GameObject iceball;
 //   Vector2 direction;
 //   public Camera cam;


    private void Awake()
    {
        IceballScript.newIceBall = iceball;
    }

    // Update is called once per frame
    private void Update()
    {
  //      Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
  //      Vector2 direction = mousePos - (Vector2)transform.position;

    //    if (Input.GetMouseButtonDown(0))
    //    {
     //       IceballScript.Charge(direction);
    //        IceballScript.OnAimChange(direction);
     //      
     //  }else if (Input.GetMouseButtonUp(0))
     ///  {
    //        IceballScript.Fire(direction);
       // }

     
    }
}
