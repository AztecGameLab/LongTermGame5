using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public Fader transition;
    public Level firstLevel;
    public Level playerLevel;
    public Level mainMenuLevel;
    
    private void Start()
    {
        LevelController.Get().LoadLevel(mainMenuLevel);
    }

    public void GoToFirstLevel()
    {
        TransitionController.Get().FadeTo(Color.black, 0.25f);
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
