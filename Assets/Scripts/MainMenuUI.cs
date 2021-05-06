using System;
using System.Linq;
using FMODUnity;
using SaveSystem;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Serializable]
    public class LevelMusic
    {
        public MusicTrigger music;
        public Level[] levels;
    }
    
    public Level mainMenuLevel;
    public Level creditsLevel;

    public GameObject quitButton;

    [SerializeField, EventRef] private string menuEnterSound;
    [SerializeField, EventRef] private string menuExitSound;
    [SerializeField, EventRef] private string menuHoverSound;
    [SerializeField] private LevelMusic[] levelMusic;
    [SerializeField] private MusicTrigger defaultMusic;
    [SerializeField] private bool shouldSetupMusic = true;

    private const string EpilogueMusicRef = "event:/Music/Epilogue Music/Epilogue Music";
    
    private void Start()
    {
        if (shouldSetupMusic)
            SetupMusic();
    }

    private void SetupMusic()
    {
        SaveLoad.LoadFromFileToTempData();
        
        var playerData = SaveLoad.GetPlayerData();
        var audioController = AudioController.Get();
        
        if (AudioController.MusicRef != EpilogueMusicRef)
            audioController.StopMusic(5);

        if (playerData.currentScene == null)
        {
            defaultMusic.Play();
        }
        else
        {
            var targetLevel = LevelController.Get().GetLevel(playerData.currentScene);
            var music = levelMusic.First(element => element.levels.Contains(targetLevel)).music;
            music.Play();
        }
    }

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
