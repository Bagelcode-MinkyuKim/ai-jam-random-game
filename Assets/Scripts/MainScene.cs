using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

        // Load all character data
        var characterDataList = Resources.LoadAll<CharacterData>("CharacterData");

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
