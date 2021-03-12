using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public class SaveListener : MonoBehaviour
    {
        private LevelController _levelController;

        private void Start()
        {
            _levelController = LevelController.Get();
            
            SceneManager.sceneLoaded += AfterLevelLoaded;
            _levelController.BeforeStartUnload += BeforeLevelUnloaded;
            _levelController.ActiveLevelChanged += OnActiveLevelChanged;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= AfterLevelLoaded;
            _levelController.BeforeStartUnload -= BeforeLevelUnloaded;
            _levelController.ActiveLevelChanged -= OnActiveLevelChanged;
        }

        private void AfterLevelLoaded(Scene scene, LoadSceneMode mode)
        {
            var level = _levelController.GetLevel(scene.name);
            
            if (level.isGameplayLevel)
                SaveLoad.LoadSceneFromTempData(scene.name);
        }

        private static void BeforeLevelUnloaded(Level level)
        {
            if (ShouldSave(level))
                SaveLoad.SaveSceneToTempData(level.sceneName);
        }
        
        private static bool ShouldSave(Level level)
        {
            return level != null && level.isGameplayLevel;
        }

        private void MoveToActiveLevel(GameObject objectToMove)
        {
            if (_levelController.ActiveLevel != null)
            {
                var activeLevel = _levelController.ActiveLevel.sceneName;
                var activeScene = SceneManager.GetSceneByName(activeLevel);
                
                SceneManager.MoveGameObjectToScene(objectToMove, activeScene);
            }
        }
        
        private void OnActiveLevelChanged(Level level)
        {
            var player = PlatformerController.instance;
            
            if (ShouldSave(level) && player != null)
            {
                MoveToActiveLevel(player.gameObject);
                
                var playerData = new PlayerData
                {
                    currentScene = level.sceneName, 
                    position = player.transform.position
                };

                SaveLoad.SetPlayerData(playerData);
            }
        }
    }    
}
