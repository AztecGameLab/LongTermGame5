using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject container;

    private void Start()
    {
        //TODO: SetVolume() get playerPref for volume
    }

    public void TogglePause()
    {
        if (container.activeInHierarchy)
            Resume();
        else
            Pause();
    }
    public void Pause()
    {
        Time.timeScale = 0;
        container.SetActive(true);
    }
    public void Resume()
    {
        container.SetActive(false);
        Time.timeScale = 1f;
    }
    public void GoToMainMenu()
    {
        int MainMenuSceneIndex = 0;
        SceneManager.LoadScene(MainMenuSceneIndex);
    }
    public void SetVolume()
    {
        Debug.LogWarning("Volume not implemented yet");
    }
}
