using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTestPlayer : SaveTestEntity, ISaveable
{

    public static SaveTestPlayer instance;
    public float mana = 200;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        //add myself to save file
    }

    public object CaptureState()
    {
        var baseSaveData = (SaveTestEntity.SaveData)base.CaptureState();

        return new SaveData(health, transform.position.x, transform.position.y, mana);
    }
    new public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        health = saveData.health;
        transform.position = new Vector2(saveData.x, saveData.y);
        mana = saveData.mana;
    }

    [Serializable]
    public class SaveData : SaveTestEntity.SaveData
    {
        public float mana;
        public SaveData(float health, float x, float y, float mana) : base(health, x, y)
        {
            this.mana = mana;
        }
    }
}
