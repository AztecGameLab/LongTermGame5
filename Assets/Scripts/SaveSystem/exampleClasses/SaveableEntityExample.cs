using UnityEngine;
using SaveSystem;

namespace KainsTestScripts
{
    //Example of a class/component that implements ISaveable and is saved to the SaveSystem
    public class SaveableEntityExample : MonoBehaviour, ISaveableComponent
    {

        public float health = 100;

        void Start()
        {
            //im a normal script that someone made. look at me gooo

        }






        #region SAVE SYSTEM
        [System.Serializable]
        protected class TestEntitySaveData : ISaveData //class that is a container for data that will be saved
        {
            public float health;
            public float x;
            public float y;

            public override string ToString()
            {
                return "health: " + health + ", position: (" + x + ", " + y + ")";
            }
        }

        public ISaveData GatherSaveData() //store current state into the SaveData class
        {
            return new TestEntitySaveData { health = health, x = transform.position.x, y = transform.position.y };
        }
        public void RestoreSaveData(ISaveData state) //receive SaveData class and set variables
        {
            var saveData = (TestEntitySaveData)state;

            health = saveData.health;
            transform.position = new Vector2(saveData.x, saveData.y);
        }
        #endregion
    }
}