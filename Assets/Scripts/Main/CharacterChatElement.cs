using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChatElement : MonoBehaviour
{
    [Header("UI Components")]
    public Text nameText;
    public Text latestMessageText;

    public Image profileImage;

    public Image notificationBackground;
    public Text notificationText;
    public Text notificationTimeText;


    [SerializeField] private CharacterData characterData;
    public CharacterData CharacterData => characterData;

    /// <summary>
    /// 캐릭터 데이터 받아서 UI 업데이트
    /// </summary>
    /// <param name="characterData">CharacterData 참조</param>
    /// 

    public void SetUp(CharacterData characterData)
    {
        if (characterData == null)
        {
            Debug.LogError("CharacterData is null!");
            return;
        }

        this.characterData = characterData;

        // 프로필 이미지
        if (profileImage != null)
        {
            byte[] pngData = File.ReadAllBytes(Application.persistentDataPath + $"/Resources/{characterData.name}.png");
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(pngData);
            Sprite sp = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );
            sp.name = name;
            tex.name = sp.name;
            characterData.CharacterSprite = sp;
            profileImage.sprite = sp;
        }
        profileImage.sprite = characterData.CharacterSprite;

        // 이름 (예시로 voiceId 사용, 필요 시 다른 값으로 변경)
        if (nameText != null)
            nameText.text = characterData.characterName;

        // 최근 메시지
        if (latestMessageText != null)
            latestMessageText.text = string.IsNullOrEmpty(characterData.latestMessage) ? string.Empty : characterData.latestMessage;

        // 알림 배지
        if (characterData.unreadMessageCount > 0)
        {
            if (notificationBackground != null)
                notificationBackground.gameObject.SetActive(true);

            if (notificationTimeText != null)
            {
                var timeSpan = System.DateTime.UtcNow - characterData.LastChatTime;
                if (timeSpan.TotalMinutes < 1)
                {
                    notificationTimeText.text = "Now";
                }
                else if (timeSpan.TotalMinutes < 60)
                {
                    int minutes = (int)timeSpan.TotalMinutes;
                    notificationTimeText.text = minutes == 1 ? "1 min ago" : $"{minutes} mins ago";
                }
                else if (timeSpan.TotalHours < 24)
                {
                    int hours = (int)timeSpan.TotalHours;
                    notificationTimeText.text = hours == 1 ? "1 hour ago" : $"{hours} hours ago";
                }
                else
                {
                    int days = (int)timeSpan.TotalDays;
                    notificationTimeText.text = days == 1 ? "1 day ago" : $"{days} days ago";
                }
            }

            if (notificationText != null)
                notificationText.text = characterData.unreadMessageCount.ToString();


        }
        else
        {
            notificationTimeText.text = string.Empty;
            if (notificationBackground != null)
                notificationBackground.gameObject.SetActive(false);
        }
    }

    public void OnChatRoomButtonDown()
    {
        GameManager.Instance.SetupChatting(characterData);
        CustomEvent.Trigger(ScreenFlow.Instance.gameObject, "OnChangeChatScreen");
    }
}