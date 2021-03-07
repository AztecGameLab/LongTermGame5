using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    //Component that is added to GameObjects that have other components (that implements ISaveable) that need to be saved
    [ExecuteInEditMode]
    public class SaveableGameObject : MonoBehaviour
    {
        [ReadOnly]
        public string id;


        void SetIDIfInActiveScene()
        {
            if (SceneManager.GetActiveScene().name == gameObject.scene.name) //if this component is in the active scene
            {
                if (System.String.IsNullOrEmpty(id)) //if i dont have an id
                {
                    id = System.Guid.NewGuid().ToString(); //give me a random id
                    EditorUtility.SetDirty(this);
                }
            }
            else
            {
                id = ""; //set my id to null if im not in the active scene (ex. if im an object in the prefab editor)
            }
        }
        void OnValidate()
        {
            SetIDIfInActiveScene();
        }

        private void Reset()
        {
            SetIDIfInActiveScene();
        }

        public Dictionary<string, SaveData> GatherComponentsSaveData()
        {
            var Dict_ComponentTypes_SaveData = new Dictionary<string, SaveData>();

            foreach (var saveableComponent in GetComponents<ISaveableComponent>())
            {
                Dict_ComponentTypes_SaveData[saveableComponent.GetType().ToString()] = saveableComponent.GatherSaveData();
            }

            return Dict_ComponentTypes_SaveData;
        }

        public void RestoreComponentsSaveData(Dictionary<string, SaveData> ComponentTypes_SaveData_Dict)
        {
            foreach (var saveableComponent in GetComponents<ISaveableComponent>())
            {
                string typeName = saveableComponent.GetType().ToString();
                if (ComponentTypes_SaveData_Dict.TryGetValue(typeName, out SaveData saveData))
                {
                    saveableComponent.RestoreSaveData(saveData);
                }
            }
        }
    }
}