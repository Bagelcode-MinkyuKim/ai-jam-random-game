using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum EaseType
{
    Linear,
    EaseInCubic,
    EaseOutCubic,
    EaseInOutCubic,
    EaseInQuart,
    EaseOutQuart,
    EaseInOutQuart,
    EaseOutBounce,
    EaseInSine,
    EaseOutSine,
    EaseInOutSine,
    EaseInExpo,
    EaseOutExpo,
    EaseInOutExpo
}

public static class TweenExtensions
{
    // ----------- Transform Extensions -----------

    public static async UniTask TweenPositionAsync(this Transform target, Vector3 to, float duration, EaseType easeType = EaseType.Linear, Action onComplete = null)
    {
        Vector3 from = target.position;
        await TweenAsync(duration, t => target.position = Vector3.Lerp(from, to, TweenEasings.GetEasing(easeType)(t)));
        target.position = to;
        onComplete?.Invoke();
    }

    public static async UniTask TweenLocalPositionAsync(this Transform target, Vector3 to, float duration, EaseType easeType = EaseType.Linear, Action onComplete = null)
    {
        Vector3 from = target.localPosition;
        await TweenAsync(duration, t => target.localPosition = Vector3.Lerp(from, to, TweenEasings.GetEasing(easeType)(t)));
        target.localPosition = to;
        onComplete?.Invoke();
    }

    public static async UniTask TweenScaleAsync(this Transform target, Vector3 to, float duration, EaseType easeType = EaseType.Linear, Action onComplete = null)
    {
        Vector3 from = target.localScale;
        await TweenAsync(duration, t => target.localScale = Vector3.Lerp(from, to, TweenEasings.GetEasing(easeType)(t)));
        target.localScale = to;
        onComplete?.Invoke();
    }

    public static async UniTask TweenRotationAsync(this Transform target, Quaternion to, float duration, EaseType easeType = EaseType.Linear, Action onComplete = null)
    {
        Quaternion from = target.rotation;
        await TweenAsync(duration, t => target.rotation = Quaternion.Slerp(from, to, TweenEasings.GetEasing(easeType)(t)));
        target.rotation = to;
        onComplete?.Invoke();
    }

    // ----------- RectTransform Extensions -----------

    public static async UniTask TweenAnchoredPositionAsync(this RectTransform target, Vector2 to, float duration, EaseType easeType = EaseType.Linear, Action onComplete = null)
    {
        Vector2 from = target.anchoredPosition;
        await TweenAsync(duration, t => target.anchoredPosition = Vector2.Lerp(from, to, TweenEasings.GetEasing(easeType)(t)));
        target.anchoredPosition = to;
        onComplete?.Invoke();
    }

    public static async UniTask TweenSizeDeltaAsync(this RectTransform target, Vector2 to, float duration, EaseType easeType = EaseType.Linear, Action onComplete = null)
    {
        Vector2 from = target.sizeDelta;
        await TweenAsync(duration, t => target.sizeDelta = Vector2.Lerp(from, to, TweenEasings.GetEasing(easeType)(t)));
        target.sizeDelta = to;
        onComplete?.Invoke();
    }

    // ----------- CanvasGroup Extensions -----------

    public static async UniTask TweenAlphaAsync(this CanvasGroup target, float to, float duration, EaseType easeType = EaseType.Linear, Action onComplete = null)
    {
        float from = target.alpha;
        await TweenAsync(duration, t => target.alpha = Mathf.Lerp(from, to, TweenEasings.GetEasing(easeType)(t)));
        target.alpha = to;
        onComplete?.Invoke();
    }

    // ----------- Color Tween (Optional for Graphic Components) -----------

    public static async UniTask TweenColorAsync(this UnityEngine.UI.Graphic target, Color to, float duration, EaseType easeType = EaseType.Linear, Action onComplete = null)
    {
        Color from = target.color;
        await TweenAsync(duration, t => target.color = Color.Lerp(from, to, TweenEasings.GetEasing(easeType)(t)));
        target.color = to;
        onComplete?.Invoke();
    }

    // ----------- Core Tween Loop -----------

    private static async UniTask TweenAsync(float duration, Action<float> onUpdate)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            onUpdate?.Invoke(t);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}

public static class TweenEasings
{
    public static Func<float, float> GetEasing(EaseType type)
    {
        return type switch
        {
            EaseType.Linear => Linear,
            EaseType.EaseInCubic => EaseInCubic,
            EaseType.EaseOutCubic => EaseOutCubic,
            EaseType.EaseInOutCubic => EaseInOutCubic,
            EaseType.EaseInQuart => EaseInQuart,
            EaseType.EaseOutQuart => EaseOutQuart,
            EaseType.EaseInOutQuart => EaseInOutQuart,
            EaseType.EaseOutBounce => EaseOutBounce,
            EaseType.EaseInSine => EaseInSine,
            EaseType.EaseOutSine => EaseOutSine,
            EaseType.EaseInOutSine => EaseInOutSine,
            EaseType.EaseInExpo => EaseInExpo,
            EaseType.EaseOutExpo => EaseOutExpo,
            EaseType.EaseInOutExpo => EaseInOutExpo,
            _ => Linear
        };
    }

    public static float Linear(float t) => t;
    public static float EaseInCubic(float t) => t * t * t;
    public static float EaseOutCubic(float t) => (t -= 1f) * t * t + 1f;
    public static float EaseInOutCubic(float t) => t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    public static float EaseInQuart(float t) => t * t * t * t;
    public static float EaseOutQuart(float t) => 1f - Mathf.Pow(1f - t, 4f);
    public static float EaseInOutQuart(float t) => t < 0.5f ? 8f * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 4f) / 2f;
    public static float EaseOutBounce(float t)
    {
        if (t < 1f / 2.75f) return 7.5625f * t * t;
        if (t < 2f / 2.75f) return 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f;
        if (t < 2.5f / 2.75f) return 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f;
        return 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f;
    }
    public static float EaseInSine(float t) => 1f - Mathf.Cos((t * Mathf.PI) / 2f);
    public static float EaseOutSine(float t) => Mathf.Sin((t * Mathf.PI) / 2f);
    public static float EaseInOutSine(float t) => -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;
    public static float EaseInExpo(float t) => t == 0f ? 0f : Mathf.Pow(2f, 10f * (t - 1f));
    public static float EaseOutExpo(float t) => t == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t);
    public static float EaseInOutExpo(float t)
    {
        if (t == 0f) return 0f;
        if (t == 1f) return 1f;
        return t < 0.5f ? Mathf.Pow(2f, 20f * t - 10f) / 2f : (2f - Mathf.Pow(2f, -20f * t + 10f)) / 2f;
    }
}