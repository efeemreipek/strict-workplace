using UnityEngine;
using DG.Tweening;
using System;

public class ScreenFadeHandler : Singleton<ScreenFadeHandler>
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration;

    public event Action OnFadeInBegin;
    public event Action OnFadeInComplete;
    public event Action OnFadeOutBegin;
    public event Action OnFadeOutComplete;

    public void FadeIn(Action onComplete = null)
    {
        OnFadeInBegin?.Invoke();
        fadeCanvasGroup.DOFade(1f, fadeDuration)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                OnFadeInComplete?.Invoke();
            });
    }
    public void FadeOut()
    {
        OnFadeOutBegin?.Invoke();
        fadeCanvasGroup.DOFade(0f, fadeDuration)
            .OnComplete(() => OnFadeOutComplete?.Invoke());
    }
}
