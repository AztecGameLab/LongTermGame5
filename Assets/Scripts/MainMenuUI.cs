using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public Level firstLevel;
    public Level playerLevel;
    public Level mainMenuLevel;

    public void GoToFirstLevel()
    {
        StartCoroutine(EnterGameCoroutine());
    }

    private IEnumerator EnterGameCoroutine()
    {
        var transitionController = TransitionController.Get();
        var levelController = LevelController.Get();
        
        transitionController.FadeTo(Color.black, 0.25f);
        yield return new WaitForSeconds(0.25f);
        
        levelController.LoadLevel(firstLevel);
        levelController.LoadLevel(playerLevel);
        yield return new WaitUntil(() => !levelController.Loading);

        transitionController.FadeFrom(Color.black, 0.25f);
        levelController.UnloadLevelAndNeighbors(mainMenuLevel);
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
