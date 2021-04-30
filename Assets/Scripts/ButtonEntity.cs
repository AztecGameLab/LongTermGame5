using System;
using System.Collections;
using EasyButtons;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEntity : Entity
{
    [Header("Button State")]
    public bool isActive = false;
    public float timerRemaining = 0f;
    public float timerResetPercent = 0f;
    
    [Header("Button Timer Settings")] 
    public bool isTimed = false;
    public bool shootingResetsTimer = false;
    public float resetTimeSeconds = 5f;
    public Events events;
    
    [Header("Button Sounds")]
    [EventRef] public string activatedSound;
    [EventRef] public string deactivatedSound;

    public override void TakeDamage(float baseDamage)
    {
        SetActive(true);
    }

    [Button]
    public void SetActive(bool active)
    {
        if (isActive == active)
        {
            if (shootingResetsTimer && active)
            {
                ResetTimer();
                RuntimeManager.PlayOneShot(activatedSound, transform.position);
            }
            return;
        }
        
        if (isTimed && active)
            ResetTimer();
        
        
        var targetSound = active ? activatedSound : deactivatedSound;
        var targetEvent = active ? events.buttonTurnOnEvent : events.buttonTurnOffEvent;
        RuntimeManager.PlayOneShot(targetSound, transform.position);
        targetEvent?.Invoke();
        isActive = active;
    }

    public void ResetTimer()
    {
        StopAllCoroutines();
        StartCoroutine(ResetButtonCoroutine());
    }

    private IEnumerator ResetButtonCoroutine()
    {
        timerRemaining = resetTimeSeconds;

        while (timerRemaining > 0)
        {
            timerRemaining = Mathf.Max(timerRemaining - Time.deltaTime, 0);
            timerResetPercent = Mathf.Clamp01(1 - timerRemaining / resetTimeSeconds);
            yield return new WaitForEndOfFrame();
        }
        SetActive(false);
    }
}

[Serializable]
public class Events
{
    public UnityEvent buttonTurnOnEvent;
    public UnityEvent buttonTurnOffEvent;    
}