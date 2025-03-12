using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChatDebugTool : MonoBehaviour
{

    public ChatDialogue dialogue;
    public ChatContainer chatContainer;
    public ChatLayout chatLayout;
    public InputField inputField;
    public void SetTextTest(string text)
    {
        dialogue.SetText(text);
    }

    public void AddLeftText(string text)
    {
        chatContainer.AddDialogue(ChatType.LeftMessage, text);
        inputField.text = "";

    }
    public void AddRightText()
    {
        chatContainer.AddDialogue(ChatType.RightMessage, inputField.text);
        inputField.text = "";

        SendGPTMessage(inputField.text).Forget();
    }
    private List<Message> messages = new List<Message>();

    private async UniTask SendGPTMessage(string message)
    {
        Message req = new Message()
        {
            content = message,
            role = "user",
        };
        messages.Add(req);

        var response = await OpenApiClient.Instance.SendRequest(messages);
        var voiceDesign = await ElevenLabsApiClient.Instance.VoiceDesignAsync("an engaging, smooth, and expressive voice characterized by warmth and clarity, offering a perfect blend of professional authority and true charm.", "an engaging, smooth, and expressive voice characterized by warmth and clarity, offering a perfect blend of professional authority and true charm.");

        var saveResponse = await ElevenLabsApiClient.Instance.SaveVoiceDesignAsync($"AIJAM_TEST{voiceDesign.previews[0].generated_voice_id}", "TEST DESCTEST DESCTEST DESCTEST DESCTEST DESCTEST DESCTEST DESCTEST DESC", voiceDesign.previews[0].generated_voice_id);

        var audio = await ElevenLabsApiClient.Instance.CreateSpeechAsync(response.content, saveResponse.voice_id);
        await PlayBase64Audio(audio);
        AddLeftText(response.content);
    }

    public AudioSource audioSource;

    public async UniTask PlayBase64Audio(byte[] audioBytes)
    {
        float[] samples = new float[audioBytes.Length / 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short sample = BitConverter.ToInt16(audioBytes, i * 2);
            samples[i] = sample / 32768f; // 16bit PCM 정규화 (-1.0f ~ 1.0f)
        }

        AudioClip audioClip = AudioClip.Create("PCMClip", samples.Length, 1, 22050, false);
        audioClip.SetData(samples, 0);
        audioSource.clip = audioClip;
        audioSource.Play();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AddRightText();
        }

    }
}
