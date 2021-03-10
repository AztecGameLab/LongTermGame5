using System.Collections;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public Level firstLevel;
    public GameObject playerPrefab;
    public Level mainMenuLevel;
    public Level creditsLevel;

    public float fadeTime = 0.25f;
    public GameObject quitButton;

    private TransitionController _transitionController;
    private LevelController _levelController;
    
    private void Start()
    {
        _transitionController = TransitionController.Get();
        _levelController = LevelController.Get();
    }

    public void EnterGame()
    {
        Level level;
        var currentPlayerScene = SaveLoad.GetPlayerCurrentScene();

        if (currentPlayerScene == null)
        {
            // No save has been made yet, load cutscene / starting scene
            level = firstLevel;
            print("No save found: Loading " + level.sceneName);
        }
        else
        {
            level = _levelController.GetLevel(currentPlayerScene);
            print("Save found! Loading " + level.sceneName);
        }

        StartCoroutine(LoadLevelCoroutine(level));
    }

    private IEnumerator LoadLevelCoroutine(Level level)
    {
        _transitionController.FadeTo(Color.black, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        
        _levelController.LoadLevel(level, true);
        yield return new WaitUntil(() => !_levelController.Loading);
        
        Instantiate(playerPrefab);
        
        _transitionController.FadeFrom(Color.black, fadeTime);
        _levelController.UnloadLevel(mainMenuLevel);
    }

    public void PlayCredits()
    {
        _levelController.LoadLevel(creditsLevel);
        _levelController.UnloadLevel(mainMenuLevel);
    }

    public void LoadMainMenu()
    {
        _levelController.LoadLevel(mainMenuLevel);
        _levelController.UnloadLevel(creditsLevel);
    }

    public void QuitProgram()
    {
        #if UNITY_EDITOR
        
        UnityEditor.EditorApplication.isPlaying = false;
        
        #else
        
        Application.Quit();

        #endif
    }

    #if UNITY_WEBGL
    private void Start()
    {
        Destroy(quitButton);
    }
    #endif
}
