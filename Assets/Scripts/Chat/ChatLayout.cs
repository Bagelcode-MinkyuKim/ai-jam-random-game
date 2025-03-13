using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class ChatLayout : MonoBehaviour
{
    [SerializeField] private ChatContainer chatContainer;
    [SerializeField] private RectTransform contents;
    [SerializeField] private RectTransform area;
    [SerializeField] private Rect margin;
    [SerializeField] private float verticalSpacing = 10f;
    // Start is called before the first frame update

    private void OnEnable()
    {
        chatContainer.onChatAdded += OnChatAdded;
    }

    private void OnDisable()
    {
        chatContainer.onChatAdded -= OnChatAdded;
    }

    private void OnChatAdded(ChatType chatType, ChatDialogue chatDialogue)
    {
        chatDialogue.transform.SetParent(contents, true);
        List<ChatDialogue> chatDialogues = chatContainer.ChatDialogues;

        chatDialogue.SetText(WrapText(chatDialogue.Text.text, area.rect.width * 0.75f, chatDialogue.Text.font, chatDialogue.Text.fontSize, 0f));
        chatDialogue.FitText();

        if (chatDialogues.Count == 1)
        {
            float y = contents.rect.yMax - margin.yMax;
            float x = 0;

            if (chatType == ChatType.LeftMessage)
                x = contents.rect.xMin + margin.xMin;
            else if (chatType == ChatType.RightMessage)
                x = contents.rect.xMax - margin.xMax - chatDialogue.Size.x;

            (chatDialogue.transform as RectTransform).anchoredPosition = new Vector3(x, y, 0f);
        }
        else
        {
            float y = (chatDialogues[chatDialogues.Count - 2].transform as RectTransform).anchoredPosition.y - (chatDialogues[chatDialogues.Count - 2].transform as RectTransform).rect.height - verticalSpacing;
            float x = 0;

            if (chatType == ChatType.LeftMessage)
                x = contents.rect.xMin + margin.xMin;
            else if (chatType == ChatType.RightMessage)
                x = contents.rect.xMax - margin.xMax - chatDialogue.Size.x;
            (chatDialogue.transform as RectTransform).anchoredPosition = new Vector3(x, y, 0f);
        }
    }

    public string WrapText(string input, float threshold, Font font, int fontSize, float letterSpacing)
    {
        font.RequestCharactersInTexture(input, fontSize, FontStyle.Normal);

        StringBuilder wrappedText = new StringBuilder();
        float currentLineWidth = 0f;

        foreach (char c in input)
        {
            if (c == '\n')
            {
                wrappedText.Append(c);
                currentLineWidth = 0f;
                continue;
            }

            CharacterInfo charInfo;
            if (!font.GetCharacterInfo(c, out charInfo, fontSize, FontStyle.Normal))
            {
                charInfo.advance = 10;
            }

            float charWidth = charInfo.advance + letterSpacing;

            if (currentLineWidth + charWidth > threshold)
            {
                wrappedText.Append('\n');
                currentLineWidth = 0f;
            }

            wrappedText.Append(c);
            currentLineWidth += charWidth;
        }

        return wrappedText.ToString();
    }
}
