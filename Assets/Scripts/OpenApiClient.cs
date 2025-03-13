using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;

public class OpenApiClient : MonoBehaviour
{
    public static OpenApiClient Instance;

    private void Awake()
    {
        Instance = this;
    }
    [SerializeField] private string apiKey = "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";
    private string apiURL = "https://api.openai.com/v1/chat/completions";

    public async UniTask<Message> SendRequest(List<Message> messages)
    {
        RequestBody requestBody = new RequestBody { model = "gpt-4o", messages = messages };
        string jsonBody = JsonUtility.ToJson(requestBody);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        UnityWebRequest request = new UnityWebRequest(apiURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        await request.SendWebRequest().ToUniTask();
        if (request.result != UnityWebRequest.Result.Success)
        {
            return null;
        }

        ChatCompletionResponse response = JsonUtility.FromJson<ChatCompletionResponse>(request.downloadHandler.text);
        return new Message()
        {
            content = response.choices[response.choices.Length - 1].message.content,
            role = "assistant"
        };
    }
}

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}

[System.Serializable]
public class RequestBody
{
    public string model;
    public List<Message> messages;
}
[System.Serializable]
public class ChatCompletionResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
}