using System;
using System.Collections;
using UnityEngine;

// Allows child-classes to safely fade a value in and out over animation curves.
public abstract class Fader : MonoBehaviour
{
    public event Action<FadeType> TransitionStartEvent;
    public event Action<FadeType> TransitionStopEvent;
    
    public AnimationCurve     fadeInCurve = AnimationCurve.Linear(0, 0, 3, 1);
    public AnimationCurve     fadeOutCurve = AnimationCurve.Linear(0, 1, 3, 0);

    protected abstract float  Value { get; set; }
    private Coroutine         CurrentCoroutine { get; set; } = null;
    public bool               Transitioning { get; protected set; } = false;
    public float              Duration { get; set; } = 1;
    
    public void Fade(FadeType type, Action onComplete = null)
    {
        if (CurrentCoroutine != null && Transitioning) // If this component is already fading, cancel the previous fade.
        {
            StopCoroutine(CurrentCoroutine);
            TransitionStopEvent?.Invoke(type);
        }

        AnimationCurve curve = type == FadeType.In ? fadeInCurve : fadeOutCurve;
        CurrentCoroutine = StartCoroutine(FadeCoroutine(curve, type, onComplete));
    }

    private IEnumerator FadeCoroutine(AnimationCurve curve, FadeType type, Action onComplete)
    {
        TransitionStartEvent?.Invoke(type);
        Transitioning = true;
        
        AnimationCurveHelper.ChangeFirstKeyframeValue(curve, Value);
        AnimationCurveHelper.ChangeLastKeyframeTime(curve, Duration);
        
        float endTime = Duration;
        float elapsedTime = 0;

        while (elapsedTime <= endTime)
        {
            Value = curve.Evaluate(elapsedTime);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Value = AnimationCurveHelper.LastKey(curve).value;
        Transitioning = false;
        onComplete?.Invoke();
        TransitionStopEvent?.Invoke(type);
    }
}

public enum FadeType
{
    In,
    Out
}