using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTestEntity : MonoBehaviour, ISaveable
{

    public float health = 100;
    Vector2 position = new Vector2(0, 0);

    private void Start()
    {

        //add myself to save file
    }


    public object CaptureState()
    {
        return new SaveData
        {
            health = this.health,
            position = new float[2] {position.x, position.y}
        };
    }
    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        health = saveData.health;
        position.x = saveData.position[0];
        position.y = saveData.position[1];
    }

    [Serializable]
    struct SaveData
    {
        public float health;
        public float[] position;
    }
}
