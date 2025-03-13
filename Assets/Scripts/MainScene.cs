using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainScene : MonoBehaviour
{
    public CharacterChatElement characterChatElementPrefab;
    public List<CharacterChatElement> characterChatElements = new List<CharacterChatElement>();
    public RectTransform chatParent;

    private void OnEnable()
    {
        LoadCharacterData();
        SoundManager.Instance.PlayLobbyBGM();
    }

    public void LoadCharacterData()
    {
        characterChatElements = new List<CharacterChatElement>();
        //chatParent delete all childs
        foreach (Transform child in chatParent)
        {
            Destroy(child.gameObject);
        }

        string directoryPath = Path.Combine(Application.persistentDataPath, "characterData");

        if (!Directory.Exists(directoryPath))
        {
            Debug.LogWarning($"❌ 캐릭터 데이터 폴더가 존재하지 않습니다: {directoryPath}");
            return;
        }

        string[] jsonFiles = Directory.GetFiles(directoryPath, "CharacterData_*.json");

        List<CharacterData> characterDataList = new List<CharacterData>();
        foreach (string filePath in jsonFiles)
        {
            string json = File.ReadAllText(filePath);
            CharacterData characterData = ScriptableObject.CreateInstance<CharacterData>();
            JsonUtility.FromJsonOverwrite(json, characterData);
            characterDataList.Add(characterData);
        }

        // Create character chat elements
        foreach (var characterData in characterDataList)
        {
            var characterChatElement = Instantiate(characterChatElementPrefab, chatParent);
            characterChatElement.SetUp(characterData);
            characterChatElements.Add(characterChatElement);
        }

        //마지막 대화시간 오름차순 정렬
        characterChatElements.Sort((a, b) => a.CharacterData.LastChatTime.CompareTo(b.CharacterData.LastChatTime));
    }


    public void OnClickGenerationButton()
    {
        CustomEvent.Trigger(ScreenFlow.Instance.gameObject, "OnChangeCharacterGenerationScreen");
    }

}
