using System;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace SaveSystem
{
    //Component that is added to GameObjects that have other components (that implements ISaveable) that need to be saved
    [ExecuteInEditMode]
    public class SaveableGameObject : MonoBehaviour
    {
        [ReadOnly]
        public string id;

#if UNITY_EDITOR
        void SetIDIfInActiveScene() //TODO doesn't give new id if you duplicate a saveable  in the scene
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
#endif

        public GameObjectData GatherComponentsSaveData()
        {
            var gameObjectData = new GameObjectData();

            foreach (var saveableComponent in GetComponents<ISaveableComponent>())
            {
                gameObjectData.dict[saveableComponent.GetType().ToString()] = saveableComponent.GatherSaveData();
            }

            return gameObjectData;
        }

        public void RestoreComponentsSaveData(GameObjectData gameObjectData)
        {
            foreach (var saveableComponent in GetComponents<ISaveableComponent>())
            {
                string typeName = saveableComponent.GetType().ToString();
                if (gameObjectData.dict.TryGetValue(typeName, out ISaveData saveData))
                {
                    saveableComponent.RestoreSaveData(saveData);
                }
            }
        }
    }
}