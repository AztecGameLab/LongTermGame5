using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public interface ISaveData
    {
        string ToString();
    }
    
    [Serializable]
    public class GameData
    {
        [SerializeField]
        public PlayerData playerData;
        [SerializeField]
        public Dictionary<string, SceneData> dict = new Dictionary<string, SceneData>();

        public GameData()
        {
            playerData = new PlayerData();
            dict = new Dictionary<string, SceneData>();
        }
    }

    [Serializable]
    public class PlayerData
    {
        [SerializeField] 
        public string currentScene;
        [SerializeField] 
        public SVector3 position;
        [SerializeField] 
        public int unlockState;
        [SerializeField] 
        public bool waterEnemyAngry;
    }

    [Serializable]
    public class SceneData
    {
        [SerializeField]
        public Dictionary<string, GameObjectData> dict = new Dictionary<string, GameObjectData>();
    }

    [Serializable]
    public class GameObjectData
    {
        [SerializeField]
        public Dictionary<string, ISaveData> dict = new Dictionary<string, ISaveData>();
    }

}