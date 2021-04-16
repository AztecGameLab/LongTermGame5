using UnityEngine;
using UnityEngine.UI;

public class SolidColorFader : Fader
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image image;
    
    protected override float Value
    {
        get => canvasGroup.alpha;
        set => canvasGroup.alpha = value;
    }

    private void Awake()
    {
        canvasGroup.blocksRaycasts = false;

        TransitionStartEvent += OnTransitionStart;
        TransitionStopEvent += OnTransitionStop;
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    private void OnTransitionStart(FadeType type)
    {
        canvasGroup.blocksRaycasts = true;
    }

    private void OnTransitionStop(FadeType type)
    {
        canvasGroup.blocksRaycasts = false;
    }
}
