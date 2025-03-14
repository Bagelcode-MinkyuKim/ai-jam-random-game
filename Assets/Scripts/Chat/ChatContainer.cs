using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ChatType
{
    LeftMessage = 0,
    RightMessage,
}
public class ChatContainer : MonoBehaviour
{
    [SerializeField] private List<ChatDialogue> chatDialogues = new List<ChatDialogue>();
    [SerializeField] private List<ChatDialogue> dialougePrefabContiner = new List<ChatDialogue>();
    public UnityAction<ChatType, ChatDialogue> onChatAdded;

    public RectTransform chatParent;

    private void Awake()
    {
        onChatAdded = new UnityAction<ChatType, ChatDialogue>((chatType, chatDialogue) => { });
    }
    public List<ChatDialogue> ChatDialogues { get { return new List<ChatDialogue>(chatDialogues); } }
    public ChatDialogue AddDialogue(ChatType directionType, string message)
    {
        var chatDialogue = Instantiate(dialougePrefabContiner[(int)directionType], chatParent);
        chatDialogue.SetText(message);
        chatDialogues.Add(chatDialogue);
        chatParent.sizeDelta += new Vector2(0, (chatDialogue.transform as RectTransform).sizeDelta.y + 50);
        onChatAdded.Invoke(directionType, chatDialogue);

        return chatDialogue;
    }

    private void OnEnable()
    {
    }



    public void Clear()
    {
        chatParent.sizeDelta = new Vector2(chatParent.sizeDelta.x, 50);
        foreach (Transform child in chatParent)
        {
            Destroy(child.gameObject);
        }
        chatDialogues.Clear();
    }
}
