using UnityEngine;
using SaveSystem;

namespace KainsTestScripts
{

    //Example of a class/component that inherits from SaveTestEntity and implements ISaveable and is saved to the SaveSystem
    public class SaveablePlayerExample : SaveableEntityExample, ISaveableComponent
    {

        public float mana = 200;

        void Start()
        {
            //im a normal script that someone made. look at me gooo

        }







        #region SAVE SYSTEM
        [System.Serializable]
        protected class TestPlayerSaveData : TestEntitySaveData //inherited class that is a container for data that will be saved
        {
            public float mana;

            public override string ToString()
            {
                return (base.ToString() + ", mana: " + mana);
            }
        }

        new public ISaveData GatherSaveData() //store current state into the SaveData class
        {
            return new TestPlayerSaveData { health = health, x = transform.position.x, y = transform.position.y, mana = mana };
        }
        new public void RestoreSaveData(ISaveData state) //receive SaveData class and set variables
        {
            var saveData = (TestPlayerSaveData)state;

            health = saveData.health;
            transform.position = new Vector2(saveData.x, saveData.y);
            mana = saveData.mana;
        }
        #endregion


    }

}