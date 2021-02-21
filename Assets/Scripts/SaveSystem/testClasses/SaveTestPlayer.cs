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
        return new SaveData
        {
            mana = this.mana
        };
    }
    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        mana = saveData.mana;
    }

    [Serializable]
    struct SaveData
    {
        public float mana;
    }

}
