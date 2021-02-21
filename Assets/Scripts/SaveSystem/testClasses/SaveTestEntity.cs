using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTestEntity : MonoBehaviour, ISaveable
{

    public float health = 100;

    void Start()
    {
        //im a normal script that someone made. look at me gooo

    }




    //SAVE SYSTEM
    [Serializable]
    protected class SaveData //class acts as a container for data that needs to be saved
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

    public object CaptureState() //store current state into the SaveData class
    {
        return new SaveData(health, transform.position.x, transform.position.y);
    }
    public void RestoreState(object state) //receive SaveData class and set data
    {
        var saveData = (SaveData)state;

        health = saveData.health;
        transform.position = new Vector2(saveData.x, saveData.y);
    }


}
