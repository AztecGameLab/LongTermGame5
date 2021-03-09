using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public class LevelSaver : MonoBehaviour
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

        private void BeforeLevelUnloaded(Level level)
        {
            if (ShouldSave(level))
            {
                SaveLoad.SaveSceneToTempData(level.sceneName);
                MovePlayerToActiveLevel();
            }
        }
        
        private static bool ShouldSave(Level level)
        {
            return level != null && level.isGameplayLevel;
        }

        private void MovePlayerToActiveLevel()
        {
            if (_levelController.ActiveLevel != null)
            {
                var player = PlatformerController.instance.gameObject;
                var activeLevel = _levelController.ActiveLevel.sceneName;
                var activeScene = SceneManager.GetSceneByName(activeLevel);
                
                SceneManager.MoveGameObjectToScene(player, activeScene);
            }
        }
        
        private static void OnActiveLevelChanged(Level level)
        {
            if (ShouldSave(level))
            {
                SaveLoad.SetPlayerCurrentScene(level.sceneName);
            }
        }
    }    
}
