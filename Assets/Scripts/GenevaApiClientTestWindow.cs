using UnityEditor;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class GenevaApiClientTestWindow : EditorWindow
{
    private string prompt = "Pretty Girl";
    private Sprite resultSprite;
    private bool isLoading = false;

    [MenuItem("Tools/Geneva API Client Test Window")]
    public static void ShowWindow()
    {
        GetWindow<GenevaApiClientTestWindow>("Geneva API Test");
    }

    private void OnGUI()
    {
        GUILayout.Label("Geneva API Test Interface", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("Prompt:");
        prompt = EditorGUILayout.TextField(prompt);

        GUILayout.Space(10);

        if (GUILayout.Button("Generate Image from API"))
        {
            GenerateImage().Forget();
        }

        GUILayout.Space(20);

        if (isLoading)
        {
            GUILayout.Label("Generating image, please wait...");
        }
        else if (resultSprite != null)
        {
            GUILayout.Label("Generated Image:");
            GUILayout.Space(10);
            Rect rect = GUILayoutUtility.GetRect(256, 256, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            EditorGUI.DrawPreviewTexture(rect, resultSprite.texture);
        }
    }

    private async UniTaskVoid GenerateImage()
    {
        isLoading = true;
        resultSprite = null;
        Repaint();


        Sprite sprite = await GenevaApiClient.Instance.SendRequest(prompt, "Test");
        resultSprite = sprite;

        isLoading = false;
        Repaint();
    }
}
