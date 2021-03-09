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
            if (_levelController.GetLevel(scene.name).isGameplayLevel)
                SaveLoad.LoadSceneFromTempData(scene.name);
        }

        private void BeforeLevelUnloaded(Level level)
        {
            if (ShouldSave(level))
                return;
            
            SaveLoad.SaveSceneToTempData(level.sceneName);
            
            if (_levelController.ActiveLevel != null)
                SceneManager.MoveGameObjectToScene(PlatformerController.instance.gameObject, SceneManager.GetSceneByName(_levelController.ActiveLevel.sceneName));
        }

        private static void OnActiveLevelChanged(Level level)
        {
            if (ShouldSave(level))
                return;
            
            SaveLoad.SetPlayerCurrentScene(level.sceneName);
        }

        private static bool ShouldSave(Level level)
        {
            return level == null || !level.isGameplayLevel;
        }
    }    
}
