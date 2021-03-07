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
        public string PlayerCurrentScene;
        [SerializeField]
        public Dictionary<string, SceneData> dict = new Dictionary<string, SceneData>();
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