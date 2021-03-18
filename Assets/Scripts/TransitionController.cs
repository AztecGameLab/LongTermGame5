using UnityEngine;

// Add more transitions in this class, if needed.
public class TransitionController : Singleton<TransitionController>
{
    [SerializeField] private SolidColorFader solidColorFader; 
    
    public void FadeTo(Color color, float time = 1)
    {
        solidColorFader.Duration = time;
        solidColorFader.SetColor(color);
        solidColorFader.Fade(FadeType.In);
    }

    public void FadeFrom(Color color, float time = 1)
    {
        solidColorFader.Duration = time;
        solidColorFader.SetColor(color);
        solidColorFader.Fade(FadeType.Out);
    }
}