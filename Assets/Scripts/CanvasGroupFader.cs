using UnityEngine;

public class CanvasGroupFader : Fader
{
    [SerializeField] private CanvasGroup canvasGroup;

    protected override float Value
    {
        get => canvasGroup.alpha;
        set => canvasGroup.alpha = value;
    }

    private void Awake()
    {
        canvasGroup.interactable = false;
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
