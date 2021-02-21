using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class SaveController : MonoBehaviour
{
    public static SaveController instance;

    public SaveGame currentSave = new SaveGame();

    private void Awake()
    {
        instance = this;
    }

    [Button]
    public void SaveGame()
    {
        currentSave.entities.Add(new SavePlayer(SaveTestPlayer.instance));
        SaveTestEntity[] entities = FindObjectsOfType<SaveTestEntity>();
        foreach (SaveTestEntity entity in entities)
        {
            
        }

        SaveSystem.SaveGame(currentSave);
    }


    public GameObject player;
    public GameObject monster1;
    public GameObject monster2;

    [Button]
    public void LoadGame()
    {
        currentSave = SaveSystem.LoadGame();

        //for every saved object
        //if it already exists update its values
        //if it dosnt, create and set its values

    }
}
