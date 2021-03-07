using SaveSystem;
using UnityEngine;

namespace KainsTestScripts
{
    public class SceneSaverExample : MonoBehaviour
    {
        void Start()
        {
            //"pretend im something that changes scenes or something idk"
        }

        [EasyButtons.Button]
        public void save()
        {
            SaveLoad.SaveCurrentScene();
        }

        [EasyButtons.Button]
        public void load()
        {
            SaveLoad.LoadCurrentScene();
        }
    }
}