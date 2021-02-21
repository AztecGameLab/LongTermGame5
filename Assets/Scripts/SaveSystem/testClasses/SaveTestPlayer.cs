using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTestPlayer : SaveTestEntity, ISaveable
{

    public float mana = 200;


    void Start()
    {
        //im a normal script that someone made. look at me gooo

    }



    //SAVE SYSTEM
    [Serializable]
    new protected class SaveData : SaveTestEntity.SaveData //inherited class acts as a container for data that needs to be saved
    {
        public float mana;
        public SaveData(float health, float x, float y, float mana) : base(health, x, y)
        {
            this.mana = mana;
        }
    }

    new public object CaptureState() //store current state into the SaveData class
    {
        var baseSaveData = (SaveTestEntity.SaveData)base.CaptureState();

        return new SaveData(health, transform.position.x, transform.position.y, mana);
    }
    new public void RestoreState(object state) //receive SaveData class and set data
    {
        var saveData = (SaveData)state;

        health = saveData.health;
        transform.position = new Vector2(saveData.x, saveData.y);
        mana = saveData.mana;
    }


}
