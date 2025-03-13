using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterGenerationScene : MonoBehaviour
{
    [SerializeField] List<CharacterGenerationPromptPopup> promptPopups = new List<CharacterGenerationPromptPopup>();
    [SerializeField] List<string> inputResults = new List<string>();
    [SerializeField] CanvasGroup allDoneCanvasGroup;

    void Start()
    {
        CharacterGenerationFlow().Forget();
    }


    public async UniTask CharacterGenerationFlow()
    {
        promptPopups.ForEach(prompt =>
   {
       prompt.gameObject.SetActive(false);
   });
        allDoneCanvasGroup.gameObject.SetActive(false);
        allDoneCanvasGroup.alpha = 0f;
        await UniTask.WaitForSeconds(1f);


        int currentIndex = 0;

        for (; currentIndex < promptPopups.Count; currentIndex++)
        {
            await Appear(promptPopups[currentIndex]);

            string inputResult = await promptPopups[currentIndex].WaitForInput();
            inputResults.Add(inputResult);

            await Disappear(promptPopups[currentIndex]);
        }
        allDoneCanvasGroup.gameObject.SetActive(true);
        allDoneCanvasGroup.alpha = 0f;
        await allDoneCanvasGroup.TweenAlphaAsync(1f, 1f, EaseType.EaseInCubic);

        await UniTask.WaitForSeconds(1f);

        await allDoneCanvasGroup.TweenAlphaAsync(0f, 1f, EaseType.EaseInCubic);

        await UniTask.WaitForSeconds(1f);


        CustomEvent.Trigger(ScreenFlow.Instance.gameObject, "OnChangeMainScreen");
    }

    private async UniTask Appear(CharacterGenerationPromptPopup promptPopup)
    {
        Vector2 from = new Vector2(1200f, 0f);
        Vector2 to = new Vector2(0f, 0f);
        float duration = 0.6f;

        promptPopup.gameObject.SetActive(true);
        promptPopup.rectTransform.anchoredPosition = from;
        await promptPopup.rectTransform.TweenAnchoredPositionAsync(to, duration, EaseType.EaseInCubic);
    }

    private async UniTask Disappear(CharacterGenerationPromptPopup promptPopup)
    {
        Vector2 from = new Vector2(0f, 0f);
        Vector2 to = new Vector2(-1200f, 0f);
        float duration = 0.6f;

        await promptPopup.rectTransform.TweenAnchoredPositionAsync(to, duration, EaseType.EaseInCubic);
        promptPopup.gameObject.SetActive(false);
    }
}
