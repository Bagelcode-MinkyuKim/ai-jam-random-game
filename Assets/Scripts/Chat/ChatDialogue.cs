
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ChatDialogue : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Vector2 margin;

    public Vector2 Size => rectTransform.sizeDelta;
    public Text Text => dialogueText;


    public void SetText(string text)
    {
        dialogueText.text = text;
    }

    public void FitText()
    {
        rectTransform.sizeDelta = new Vector2(dialogueText.preferredWidth, dialogueText.preferredHeight) + margin;
        dialogueText.rectTransform.sizeDelta = new Vector2(-margin.x, -margin.y);
        // dialogueText.rectTransform.localPosition = new Vector2(0, 0);
    }


}
