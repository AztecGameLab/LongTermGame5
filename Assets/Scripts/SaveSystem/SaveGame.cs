using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGame
{
    public SavePlayer player;
    public List<SaveEntity> entities;

    public SaveGame()
    {
        
    }
}