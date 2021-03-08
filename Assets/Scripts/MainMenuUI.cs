using System.Collections;
using SaveSystem;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public Level firstLevel;
    public Level playerLevel;
    public Level mainMenuLevel;
    public Level creditsLevel;

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
        StartCoroutine(EnterGameCoroutine());
    }

    private IEnumerator EnterGameCoroutine()
    {
        _transitionController.FadeTo(Color.black, 0.25f);
        yield return new WaitForSeconds(0.25f);
        
        _levelController.LoadLevel(firstLevel);
        yield return new WaitUntil(() => !_levelController.Loading);

        _transitionController.FadeFrom(Color.black, 0.25f);
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
