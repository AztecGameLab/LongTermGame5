using SaveSystem;
using UnityEngine;
using EasyButtons;

namespace KainsTestScripts
{
    public class SceneSaverExample : MonoBehaviour
    {
        void Start()
        {
            //"pretend im something that changes scenes or something idk"
        }

        [Button]
        public void saveToTemp()
        {
            SaveLoad.SaveActiveSceneToTempData();
        }
        
        [Button]
        public void loadFromTemp()
        {
            SaveLoad.LoadActiveSceneFromTempData();
        }

        [Button]
        public void saveTempToFile()
        {
            SaveLoad.SaveTempDataToFile();
        }
        
        [Button]
        public void loadFromFile()
        {
            SaveLoad.LoadFromFileToTempData();
        }
    }
}