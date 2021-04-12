using FMODUnity;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField, EventRef] private string musicEvent;
    [SerializeField] private bool playOnAwake = false;
    [SerializeField] private float transitionTime = 0f;

    private AudioController _audioController;
    
    private void Start()
    {
        _audioController = AudioController.Get();
        
        if (playOnAwake)
            Play();
    }

    public void Play()
    {
        _audioController.PlayMusic(musicEvent, transitionTime);
    }
}