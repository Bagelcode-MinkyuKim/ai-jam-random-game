using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [Header("Basic Info")]
    public Sprite CharacterSprite;
    public string voiceId;
    public string characterName;

    [Header("Prompts")]
    [TextArea] public string characterPrompt;
    [TextArea] public string outfitPrompt;
    [TextArea] public string personalityPrompt;
    [TextArea] public string VoicePrompt;

    [Header("Chat Histories")]
    public List<string> chatHistory = new List<string>();
    public List<string> playerChatHistroy = new List<string>();
    public List<string> characterChatHistory = new List<string>();

    [Header("Last Chat Info")]
    [SerializeField]
    private string lastChatTimeRaw;

    public System.DateTime LastChatTime
    {
        get => System.DateTime.TryParse(lastChatTimeRaw, out var date) ? date : System.DateTime.MinValue;
        set => lastChatTimeRaw = value.ToString("o"); // ISO 8601 format
    }

    [Header("Status")]
    public int unreadMessageCount;
    [TextArea] public string latestMessage;

    // ----------- Clone 함수 추가 -----------

    public static CharacterData CloneWithNewPromptsAndBasicInfo(Sprite newSprite, string newName, string newVoiceId, string newCharacterPrompt, string newOutfitPrompt, string newPersonalityPrompt, string newVoicePrompt)
    {
        CharacterData clone = ScriptableObject.CreateInstance<CharacterData>();

        // Basic Info 복사
        clone.CharacterSprite = newSprite;
        clone.voiceId = newVoiceId;
        clone.characterName = newName;

        // Prompt 복사
        clone.characterPrompt = newCharacterPrompt;
        clone.outfitPrompt = newOutfitPrompt;
        clone.personalityPrompt = newPersonalityPrompt;
        clone.VoicePrompt = newVoicePrompt;

        // 나머지 초기화 (채팅 기록, 상태)
        clone.chatHistory = new List<string>();
        clone.playerChatHistroy = new List<string>();
        clone.characterChatHistory = new List<string>();
        clone.unreadMessageCount = 0;
        clone.latestMessage = string.Empty;
        clone.lastChatTimeRaw = System.DateTime.UtcNow.ToString("o");

        //에셋 저장
        string path = "Assets/Resources/CharacterData/" + newName + ".asset";
        UnityEditor.AssetDatabase.CreateAsset(clone, path);
        UnityEditor.AssetDatabase.SaveAssets();

        return clone;
    }
}