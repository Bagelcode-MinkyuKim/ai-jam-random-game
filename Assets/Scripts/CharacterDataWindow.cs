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
        if (GUILayout.Button("Clone With New Prompts And Basic Info"))
        {
            CloneWithNewPromptsAndBasicInfo(promptText, basicInfo);
        }

        GUILayout.Space(20);

        // 결과 출력
        GUILayout.Label("Result", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(result, GUILayout.Height(100));
    }

    // 사용자가 원하는 형식의 메서드
    private void CloneWithNewPromptsAndBasicInfo(string prompt, string basic)
    {
        if (string.IsNullOrEmpty(prompt) || string.IsNullOrEmpty(basic))
        {
            result = "⚠️ Prompt와 Basic Info를 모두 입력하세요.";
            return;
        }

        // 원하는 클론 작업 처리 (예제: 단순 문자열 결합)
        result = $"✅ 복제 완료!\n\n[Prompt]: {prompt}\n[Basic Info]: {basic}";

        // 이후 확장 예시 (비활성화 상태)
        // SaveToFile(prompt, basic);
        // CreateScriptableObject(prompt, basic);
    }

    // 예제 확장 (파일 저장, 필요 시 사용)
    private void SaveToFile(string prompt, string basic)
    {
        string path = "Assets/ClonedData.txt";
        string content = $"Prompt: {prompt}\nBasic Info: {basic}";
        System.IO.File.WriteAllText(path, content);
        AssetDatabase.Refresh();
    }

    // 예제 확장 (ScriptableObject 생성, 필요 시 사용)
    // private void CreateScriptableObject(string prompt, string basic)
    // {
    //     var data = ScriptableObject.CreateInstance<MyDataObject>();
    //     data.prompt = prompt;
    //     data.basicInfo = basic;
    //     AssetDatabase.CreateAsset(data, "Assets/NewDataObject.asset");
    //     AssetDatabase.SaveAssets();
    //     AssetDatabase.Refresh();
    // }
}