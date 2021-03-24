using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Entity
{
    [Header("Destructible From: ")]
    [SerializeField] bool TheLeft = false;
    [SerializeField] bool TheRight = false;
    [SerializeField] bool Up = false;
    [SerializeField] bool Down = false;


    [EasyButtons.Button]
    public override void TakeDamage(float baseDamage, Vector2 direction)
    {

        if ((TheLeft && direction.x < 0)|| (TheRight && direction.x > 0) || (Down && direction.y < 0) || (Up && direction.y > 0))
        {
            Destroy(gameObject);
        }

    }

    public override void TakeDamage(float baseDamage)
    {
        
    }


}
