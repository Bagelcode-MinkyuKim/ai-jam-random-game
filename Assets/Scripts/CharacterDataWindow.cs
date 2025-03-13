using UnityEditor;
using UnityEngine;

public class CharacterDataWindow : EditorWindow
{
    private string promptText = "Enter your prompt here...";
    private string basicInfo = "Enter basic info here...";
    private string result = "";

    [MenuItem("Tools/Clone With New Prompts")]
    public static void ShowWindow()
    {
        GetWindow<CharacterDataWindow>("Clone With New Prompts");
    }

    private void OnGUI()
    {
        GUILayout.Label("Clone With New Prompts & Basic Info", EditorStyles.boldLabel);

        GUILayout.Space(10);

        // 프롬프트 입력
        GUILayout.Label("Prompt", EditorStyles.label);
        promptText = EditorGUILayout.TextField(promptText);

        GUILayout.Space(5);

        // 기본 정보 입력
        GUILayout.Label("Basic Info", EditorStyles.label);
        basicInfo = EditorGUILayout.TextField(basicInfo);

        GUILayout.Space(10);

        // 복사 버튼
        if (GUILayout.Button("Clone With New Data"))
        {
            CloneWithNewData();
        }

        GUILayout.Space(20);

        // 결과 출력
        GUILayout.Label("Result", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(result, GUILayout.Height(100));
    }

    private void CloneWithNewData()
    {
        if (string.IsNullOrEmpty(promptText) || string.IsNullOrEmpty(basicInfo))
        {
            result = "⚠️ Prompt와 Basic Info를 모두 입력하세요.";
            return;
        }

        // 예제: 단순 문자열 합치기 (여기서 필요한 실제 클론 로직으로 교체 가능)
        result = $"✅ 복제 완료!\n\n[Prompt]: {promptText}\n[Basic Info]: {basicInfo}";

        // 실제 데이터 생성 또는 복사 로직을 여기에 추가 가능
        // 예: ScriptableObject 클론, JSON 생성, 에셋 복사 등
    }
}