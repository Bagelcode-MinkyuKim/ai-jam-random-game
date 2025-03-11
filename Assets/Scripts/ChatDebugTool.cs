using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
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
        AddLeftText(response.content);
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
