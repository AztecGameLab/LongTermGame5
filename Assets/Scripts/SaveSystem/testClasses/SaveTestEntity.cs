using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTestEntity : MonoBehaviour, ISaveable
{

    public float health = 100;

    private void Start()
    {

        //add myself to save file
    }


    public object CaptureState()
    {
        return new SaveData(health, transform.position.x, transform.position.y);
    }
    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        health = saveData.health;
        transform.position = new Vector2(saveData.x, saveData.y);
    }

    [Serializable]
    public class SaveData
    {
        public float health;
        public float x;
        public float y;
        public SaveData(float health, float x, float y)
        {
            this.health = health;
            this.x = x;
            this.y = y;
        }
    }
}
