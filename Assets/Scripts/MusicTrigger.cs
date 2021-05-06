using FMODUnity;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField, EventRef] private string audioEvent;
    [SerializeField] private bool isAmbience = false;
    [SerializeField] private bool playOnAwake = false;
    [SerializeField] private bool stopAllOnAwake = false;
    [SerializeField] private float transitionTime = 0f;
    

    private AudioController _audioController;
    
    private void Start()
    {
        if (stopAllOnAwake)
        {
            _audioController = AudioController.Get();
            _audioController.StopMusic(transitionTime);
            _audioController.StopAmbience(transitionTime);
        }
        
        if (playOnAwake)
            Play();
    }

    public void Play()
    {
        if (_audioController == null)
            _audioController = AudioController.Get();
        
        if (isAmbience)
            _audioController.PlayAmbience(audioEvent, transitionTime);
        else
        {
            _audioController.PlayMusic(audioEvent, transitionTime);
        }
    }
}