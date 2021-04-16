using System;
using System.Collections;
using UnityEngine;

// Add more transitions in this class, if needed.
public class TransitionController : Singleton<TransitionController>
{
    [SerializeField] private SolidColorFader solidColorFader; 
    
    public void FadeTo(Color color, float time = 1, Action onComplete = null)
    {
        solidColorFader.Duration = time;
        solidColorFader.SetColor(color);
        solidColorFader.Fade(FadeType.In);
        
        StartCoroutine(WaitForFinish(solidColorFader, onComplete));
    }

    public void FadeFrom(Color color, float time = 1, Action onComplete = null)
    {
        solidColorFader.Duration = time;
        solidColorFader.SetColor(color);
        solidColorFader.Fade(FadeType.Out);
        
        StartCoroutine(WaitForFinish(solidColorFader, onComplete));
    }
    
    private static IEnumerator WaitForFinish(Fader fader, Action onComplete)
    {
        yield return new WaitWhile(() => fader.Transitioning);
        
        onComplete?.Invoke();
    }
}