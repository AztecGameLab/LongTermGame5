using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulkyEnemyEvent : MonoBehaviour
{
    public BulkyEnemy BulkyEnemy;

    public void AttackDone()
    {
        BulkyEnemy.AttackEnd();
    }
}
