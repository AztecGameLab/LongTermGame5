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
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    protected override void OnTransitionStart(FadeType type)
    {
        base.OnTransitionStart(type);
        canvasGroup.blocksRaycasts = true;
        print("Fade started: " + type);
    }

    protected override void OnTransitionStop(FadeType type)
    {
        base.OnTransitionStop(type);
        canvasGroup.blocksRaycasts = false;
        print("Fade ended: " + type);
    }
}
