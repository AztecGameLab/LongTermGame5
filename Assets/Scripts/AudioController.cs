using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    private class MusicData
    {
        public EventInstance Instance;
        public string EventRef;
    }
    
    private static MusicData _activeMusic;
    private static MusicData _activeAmbience;

    public static string MusicRef => _activeMusic?.EventRef;
    
    public void PlayAmbience(string eventRef, float transitionTime = 0f)
    {
        if (_activeAmbience != null && _activeAmbience.EventRef == eventRef)
            return;
        
        var musicInstance = RuntimeManager.CreateInstance(eventRef);
        StopAmbience(transitionTime, () => musicInstance.start());
        _activeAmbience = new MusicData { Instance = musicInstance, EventRef = eventRef };
    }
    
    public void StopAmbience(float fadeTime, Action onComplete = null)
    {
        if (_activeAmbience == null || !_activeAmbience.Instance.isValid())
        {
            onComplete?.Invoke();
            return;
        }
        
        StartCoroutine(FadeOutAmbience(fadeTime, onComplete));
        _activeAmbience = null;
    }
    
    public void PlayMusic(string eventRef, float transitionTime = 0f)
    {
        if (_activeMusic != null && _activeMusic.EventRef == eventRef)
            return;
        
        var musicInstance = RuntimeManager.CreateInstance(eventRef);
        StopMusic(transitionTime, () => musicInstance.start());
        _activeMusic = new MusicData { Instance = musicInstance, EventRef = eventRef };
    }

    public void StopMusic(float fadeTime, Action onComplete = null)
    {
        if (_activeMusic == null || !_activeMusic.Instance.isValid())
        {
            onComplete?.Invoke();
            return;
        }
        
        StartCoroutine(FadeOutMusic(fadeTime, onComplete));
        _activeMusic = null;
    }

    private static IEnumerator FadeOutMusic(float fadeTime, Action onComplete)
    {
        var fadingMusic = _activeMusic.Instance;
        var startTime = Time.time;
        fadingMusic.getVolume(out var startVolume);
        
        while (Time.time - startTime < fadeTime)
        {
            var percentComplete = (Time.time - startTime) / fadeTime;
            var targetVolume = Mathf.Lerp(startVolume, 0, percentComplete);
            
            fadingMusic.setVolume(targetVolume);
            yield return new WaitForEndOfFrame();
        }

        fadingMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fadingMusic.release();
        
        onComplete?.Invoke();
    }
    
    private static IEnumerator FadeOutAmbience(float fadeTime, Action onComplete)
    {
        var fadingAmbience = _activeAmbience.Instance;
        var startTime = Time.time;
        fadingAmbience.getVolume(out var startVolume);
        
        while (Time.time - startTime < fadeTime)
        {
            var percentComplete = (Time.time - startTime) / fadeTime;
            var targetVolume = Mathf.Lerp(startVolume, 0, percentComplete);
            
            fadingAmbience.setVolume(targetVolume);
            yield return new WaitForEndOfFrame();
        }

        fadingAmbience.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fadingAmbience.release();
        
        onComplete?.Invoke();
    }
}