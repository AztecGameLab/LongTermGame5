using UnityEngine;

//Example of a class/component that inherits from SaveTestEntity and implements ISaveable and is saved to the SaveSystem
public class TestPlayer : TestEntity, ISaveableComponent
{

    public float mana = 200;

    void Start()
    {
        //im a normal script that someone made. look at me gooo

    }







    //SAVE SYSTEM
    [System.Serializable]
    protected class TestPlayerSaveData : TestEntitySaveData //inherited class that is a container for data that will be saved
    {
        public float mana;
    }

    new public SaveData GatherSaveData() //store current state into the SaveData class
    {
        return new TestPlayerSaveData { health = health, x = transform.position.x, y = transform.position.y, mana = mana };
    }
    new public void RestoreSaveData(SaveData state) //receive SaveData class and set variables
    {
        var saveData = (TestPlayerSaveData)state;

        health = saveData.health;
        transform.position = new Vector2(saveData.x, saveData.y);
        mana = saveData.mana;
    }


}
