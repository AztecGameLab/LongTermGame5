using FMODUnity;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public Level mainMenuLevel;
    public Level creditsLevel;

    public GameObject quitButton;

    [SerializeField, EventRef] private string menuEnterSound;
    [SerializeField, EventRef] private string menuExitSound;
    [SerializeField, EventRef] private string menuHoverSound;

    public void OnButtonHover()
    {
        RuntimeManager.PlayOneShot(menuHoverSound);
    }
    
    public void EnterGame()
    {
        RuntimeManager.PlayOneShot(menuEnterSound);
        LevelUtil.Get().LoadSavedGame();
    }

    public void PlayCredits()
    {
        LevelUtil.Get().TransitionTo(creditsLevel);
        RuntimeManager.PlayOneShot(menuEnterSound);
    }

    public void LoadMainMenu()
    {
        LevelUtil.Get().TransitionTo(mainMenuLevel);
        RuntimeManager.PlayOneShot(menuExitSound);
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
