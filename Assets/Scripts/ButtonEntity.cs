using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEntity : Entity
{
    [Header("Button Settings")]
    public bool isActive = false;
    public bool isTimed = false;
    public bool shootingResetsTimer = false;
    public float resetTimeSeconds = 5f;
    public Events events;

    public override void TakeDamage(float baseDamage)
    {
        SetActive(true);
    }

    public void SetActive(bool active)
    {
        if (isActive == active)
        {
            if (shootingResetsTimer)
                ResetTimer();
                
            return;
        }

        if (isTimed && active)
            ResetTimer();
        
        var targetEvent = active ? events.buttonTurnOnEvent : events.buttonTurnOffEvent;
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
        yield return new WaitForSeconds(resetTimeSeconds);
        SetActive(false);
    }
}

[Serializable]
public class Events
{
    public UnityEvent buttonTurnOnEvent;
    public UnityEvent buttonTurnOffEvent;    
}