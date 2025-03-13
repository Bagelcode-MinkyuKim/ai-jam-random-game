using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGenerationPromptPopup : MonoBehaviour
{
    public RectTransform rectTransform;

    public InputField inputField;
    public Button submitButton;

    private bool finishInput = false;

    private void OnDisable()
    {
        inputField.onSubmit.RemoveAllListeners();
        submitButton.onClick.RemoveAllListeners();
        inputField.onValueChanged.RemoveAllListeners();
    }

    private void OnSubmitButtonDown()
    {
        // validation
        if (string.IsNullOrEmpty(inputField.text))
        {
            finishInput = false;
        }
        finishInput = true;
    }

    private void OnSubmit(string text)
    {
        // validation
        if (string.IsNullOrEmpty(text))
        {
            finishInput = false;
        }
        finishInput = true;
    }

    public async UniTask<string> WaitForInput()
    {
        finishInput = false;
        inputField.onSubmit.AddListener(OnSubmit);
        submitButton.onClick.AddListener(OnSubmitButtonDown);
        inputField.onValidateInput += delegate (string input, int charIndex, char addedChar) { SoundManager.Instance.PlayType(); return addedChar; };

        await UniTask.WaitUntil(() => finishInput);

        inputField.onSubmit.RemoveAllListeners();
        submitButton.onClick.RemoveAllListeners();

        return inputField.text;
    }
}
