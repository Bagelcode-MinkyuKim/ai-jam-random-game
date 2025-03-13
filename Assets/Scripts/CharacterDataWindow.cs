using UnityEditor;
using UnityEngine;

public class CharacterDataWindow : EditorWindow
{
    private Sprite newSprite;
    private string newVoiceId = "New Voice ID";
    private string newCharacterPrompt = "New Character Prompt";
    private string newOutfitPrompt = "New Outfit Prompt";
    private string newPersonalityPrompt = "New Personality Prompt";
    private string newVoicePrompt = "New Voice Prompt";

    [MenuItem("Tools/Character Data/Clone With New Prompts And Basic Info")]
    public static void ShowWindow()
    {
        GetWindow<CharacterDataWindow>("Clone Character Data");
    }

    private void OnGUI()
    {
        GUILayout.Label("🧬 CharacterData 복제 툴", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // Sprite 입력
        GUILayout.Label("Character Sprite", EditorStyles.label);
        newSprite = (Sprite)EditorGUILayout.ObjectField(newSprite, typeof(Sprite), false);

        GUILayout.Space(5);

        // Voice ID
        GUILayout.Label("Voice ID", EditorStyles.label);
        newVoiceId = EditorGUILayout.TextField(newVoiceId);

        GUILayout.Space(5);

        // Character Prompt
        GUILayout.Label("Character Prompt", EditorStyles.label);
        newCharacterPrompt = EditorGUILayout.TextArea(newCharacterPrompt, GUILayout.Height(50));

        GUILayout.Space(5);

        // Outfit Prompt
        GUILayout.Label("Outfit Prompt", EditorStyles.label);
        newOutfitPrompt = EditorGUILayout.TextArea(newOutfitPrompt, GUILayout.Height(50));

        GUILayout.Space(5);

        // Personality Prompt
        GUILayout.Label("Personality Prompt", EditorStyles.label);
        newPersonalityPrompt = EditorGUILayout.TextArea(newPersonalityPrompt, GUILayout.Height(50));

        GUILayout.Space(5);

        // Voice Prompt
        GUILayout.Label("Voice Prompt", EditorStyles.label);
        newVoicePrompt = EditorGUILayout.TextArea(newVoicePrompt, GUILayout.Height(50));

        GUILayout.Space(20);

        // 복제 버튼
        if (GUILayout.Button("🚀 Clone With New Prompts And Basic Info"))
        {
            CloneWithNewPromptsAndBasicInfo();
        }
    }

    private void CloneWithNewPromptsAndBasicInfo()
    {
        // 필수 값 체크
        if (newSprite == null)
        {
            EditorUtility.DisplayDialog("에러", "⚠️ Sprite를 설정하세요.", "확인");
            return;
        }

        // 복제 실행
        CharacterData clonedData = CharacterData.CloneWithNewPromptsAndBasicInfo(
            newSprite,
            "Test",
            newVoiceId,
            newCharacterPrompt,
            newOutfitPrompt,
            newPersonalityPrompt,
            newVoicePrompt
        );

        // 완료 안내
        EditorUtility.DisplayDialog("성공", $"✅ 복제가 완료되었습니다!\n\n경로: Assets/Resources/CharacterData/{newSprite.name}.asset", "확인");

        // 생성된 에셋 하이라키 선택
        Selection.activeObject = clonedData;
    }
}