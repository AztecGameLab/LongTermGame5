using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveEntity
{
    public float health;
    public float[] position;

    public SaveEntity(SaveTestEntity entity)
    {
        health = entity.health;

        position = new float[2];
        position[0] = entity.transform.position.x;
        position[1] = entity.transform.position.y;
    }
}
