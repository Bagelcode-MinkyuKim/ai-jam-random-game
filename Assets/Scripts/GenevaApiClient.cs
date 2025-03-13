using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;

public class GenevaApiClient : MonoBehaviour
{
    public static GenevaApiClient Instance;

    private void Awake()
    {
        Instance = this;
    }

    private string apiURL = "http://api.genevagamestudio.com:3080/images/txt2img";

    private static readonly HttpClient client = new HttpClient();

    public async UniTask<Sprite> SendRequest(string prompt, string name)
    {
        TextToImageRequestBody requestBody = new TextToImageRequestBody
        {
            checkpoint = "SD1.5/astranime_V6.safetensors",
            vae = "SD1.5/vae-ft-mse-840000-ema-pruned.safetensors",
            sampler = "dpmpp_2m",
            scheduler = "karras",
            cfg = 7,
            steps = 20,
            width = 896,
            height = 504,
            positive_prompt = $"({prompt}:1.4), (best quality:1.1), (no_human:1.3), (background:1.2), (indoors:1.1)",
            negative_prompt = "(low quality:1.2), blurry, (nsfw:1.2), (human:1.4), (people:1.4)",
            batch_size = 1
        };
        string jsonBody = JsonUtility.ToJson(requestBody);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        UnityWebRequest request = new UnityWebRequest(apiURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("User-Agent", "curl/7.68.0");
        request.downloadHandler = new DownloadHandlerBuffer();
        await request.SendWebRequest().ToUniTask();
        if (request.result != UnityWebRequest.Result.Success)
        {
            return null;
        }

        string responseText = request.downloadHandler.text;

        var response = JsonUtility.FromJson<TextToImageRequestResponse>(responseText);
        return await DownloadImageAsync(response.image_path, name);
    }

    private async UniTask<Sprite> DownloadImageAsync(string url, string name)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://cdn.genevagamestudio.com/" + url);
        await request.SendWebRequest().ToUniTask();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"이미지 다운로드 실패: {request.error}");
            return null;
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
            sprite.name = "GeneratedImage";

            // Save to Resources
            SaveTextureToResources(texture, name);
            byte[] pngData = File.ReadAllBytes(Application.persistentDataPath + $"/Resources/{name}.png");
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(pngData);
            Sprite sp = Sprite.Create(
                tex,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
            sp.name = name;
            tex.name = sp.name;
            return sp; // UI Image에 적용
        }
    }

    private void SaveTextureToResources(Texture2D texture, string name)
    {
        // Create directory if it doesn't exist
        string directoryPath = Application.persistentDataPath + $"/Resources/";

        // Convert texture to PNG
        byte[] pngData = texture.EncodeToPNG();

        if (pngData != null)
        {
            // Save PNG file
            string filePath = System.IO.Path.Combine(directoryPath, name + ".png");
            System.IO.File.WriteAllBytes(filePath, pngData);
            Debug.Log($"✅ 텍스처 저장 완료: {filePath}");
        }
        else
        {
            Debug.LogError("❌ PNG 변환 실패");
        }
    }
}
[System.Serializable]
public class TextToImageRequestResponse
{
    public string image_path;
    public int width;
    public int height;
}
[System.Serializable]
public class TextToImageRequestBody
{
    public int seed = 0;
    public string positive_prompt = "";
    public string negative_prompt = "";
    public string checkpoint = "SD1.5/astranime_V6.safetensors";
    public string sampler = "euler";
    public string scheduler = "normal";
    public int steps = 20;
    public int cfg = 7;
    public int width = 512;
    public int height = 512;
    public int batch_size = 1;
    public string vae = "SD1.5/vae-ft-mse-840000-ema-pruned.safetensors";
    public string pose_b64 = "";
    public List<string> loras = new List<string>(); // 빈 리스트로 초기화
    public bool remove_background = false;
}
