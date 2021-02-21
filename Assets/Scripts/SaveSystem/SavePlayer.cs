using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavePlayer : SaveEntity
{
    public float mana;

    public SavePlayer(SaveTestPlayer player) : base ((SaveTestEntity)player)
    {
        mana = player.mana;
    }
}