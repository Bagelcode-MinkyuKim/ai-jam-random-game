using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class MathUtility
{
    public static float EaseInCubic(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t * t + b;
    }

    public static float EaseOutCubic(float t, float b, float c, float d)
    {
        t = t / d - 1;
        return c * (t * t * t + 1) + b;
    }

    public static float EaseInOutCubic(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t * t + b;
        t -= 2;
        return c / 2 * (t * t * t + 2) + b;
    }

    public static float EaseInQuart(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t * t * t + b;
    }

    public static float EaseOutQuart(float t, float b, float c, float d)
    {
        t = t / d - 1;
        return -c * (t * t * t * t - 1) + b;
    }

    public static float EaseInOutQuart(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t * t * t + b;
        t -= 2;
        return -c / 2 * (t * t * t * t - 2) + b;
    }

    public static float EaseInQuint(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t * t * t * t + b;
    }

    public static float EaseOutQuint(float t, float b, float c, float d)
    {
        t = t / d - 1;
        return c * (t * t * t * t * t + 1) + b;
    }

    public static float EaseInOutQuint(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t * t * t * t + b;
        t -= 2;
        return c / 2 * (t * t * t * t * t + 2) + b;
    }

    public static float EaseOutBounce(float t, float b, float c, float d)
    {
        t /= d;
        if (t < (1 / 2.75f))
        {
            return c * (7.5625f * t * t) + b;
        }
        else if (t < (2 / 2.75f))
        {
            t -= (1.5f / 2.75f);
            return c * (7.5625f * t * t + 0.75f) + b;
        }
        else if (t < (2.5 / 2.75))
        {
            t -= (2.25f / 2.75f);
            return c * (7.5625f * t * t + 0.9375f) + b;
        }
        else
        {
            t -= (2.625f / 2.75f);
            return c * (7.5625f * t * t + 0.984375f) + b;
        }
    }
}
