using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void GoToFirstLevel()
    {
        int firstLevelSceneIndex = 2;
        SceneManager.LoadScene(firstLevelSceneIndex);
    }

    public void GoToCredits()
    {
        int CreditsSceneIndex = 1;
        SceneManager.LoadScene(CreditsSceneIndex);
    }

    public void GoToMainMenu()
    {
        int mainMenuSceneIndex = 0;
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void QuitProgram()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public GameObject quitButton;
#if UNITY_WEBGL
    private void Start()
    {
        Destroy(quitButton);
    }
#endif
}
