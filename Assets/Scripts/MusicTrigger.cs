using FMODUnity;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField, EventRef] private string audioEvent;
    [SerializeField] private bool isAmbience = false;
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
        if (isAmbience)
            _audioController.PlayAmbience(audioEvent, transitionTime);
        else
        {
            _audioController.PlayMusic(audioEvent, transitionTime);
        }
    }
}