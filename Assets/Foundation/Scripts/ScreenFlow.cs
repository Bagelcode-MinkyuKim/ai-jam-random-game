using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenFlow : MonoBehaviour
{
    public static ScreenFlow Instance { get; private set; }
    private ScriptMachine scriptMachine;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        scriptMachine = GetComponent<ScriptMachine>();

    }

    // public static Coroutine FadeScreen(ScreenFlow screenFlow, GameObject before, GameObject after, float duration)
    // {
    //     return screenFlow.StartCoroutine(screenFlow.FadeScreenCoroutine(before, after, duration));
    // }
    // public Coroutine FadeScreen(GameObject before, GameObject after, float duration)
    // {
    //     return StartCoroutine(FadeScreenCoroutine(before, after, duration));
    // }
    public void FadeScreen(GameObject before, GameObject after, float duration)
    {
        StartCoroutine(FadeScreenCoroutine(before, after, duration));
    }
    public IEnumerator FadeScreenCoroutine(GameObject before, GameObject after, float duration)
    {
        float currentTime = 0f;
        after.SetActive(true);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            before.GetComponent<CanvasGroup>().alpha = 1f - MathUtility.EaseInCubic(currentTime, 0f, 1f, duration);
            after.GetComponent<CanvasGroup>().alpha = MathUtility.EaseInCubic(currentTime, 0f, 1f, duration);
            yield return null;
        }
        before.SetActive(false);
    }
}