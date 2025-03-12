using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using MemoryPack;
[MemoryPackable]
public partial class ElevenLabsApiClient : MonoBehaviour
{
    public static ElevenLabsApiClient Instance;
    private void Awake()
    {
        Instance = this;
    }
    [SerializeField] private string apiKey;
    private string apiBaseUrl = "https://api.elevenlabs.io/v1";
    public string DefaultVoiceId { get; set; }

    [Serializable]
    public class VoiceSettings
    {
        public float stability;
        public float similarity_boost;
        public float style;
        public bool use_speaker_boost;
        public float speed;
        public VoiceSettings() { }
        public VoiceSettings(float stability, float similarityBoost, float style = 0f, bool useSpeakerBoost = true, float speed = 1f)
        {
            this.stability = stability;
            this.similarity_boost = similarityBoost;
            this.style = style;
            this.use_speaker_boost = useSpeakerBoost;
            this.speed = speed;
        }
    }

    public async UniTask<byte[]> CreateSpeechAsync(string text, string voiceId = null)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("ElevenLabs API key is not set.");
            throw new InvalidOperationException("API key not configured.");
        }
        string chosenVoiceId = voiceId ?? DefaultVoiceId;
        if (string.IsNullOrEmpty(chosenVoiceId))
        {
            Debug.LogError("No voice ID specified for text-to-speech.");
            throw new ArgumentException("Voice ID must be provided either as a parameter or set as DefaultVoiceId.");
        }
        string url = $"{apiBaseUrl}/text-to-speech/{chosenVoiceId}?output_format=pcm_22050";
        var requestPayload = new TTSRequest { text = text, model_id = "eleven_multilingual_v2" };
        string jsonData = JsonUtility.ToJson(requestPayload);
        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            request.SetRequestHeader("xi-api-key", apiKey);
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            await request.SendWebRequest().ToUniTask();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"TTS API request failed: {request.error}");
                return null;
            }
            byte[] audioData = request.downloadHandler.data;
            if (audioData == null || audioData.Length == 0)
            {
                Debug.LogWarning("TTS API returned an empty audio response.");
            }
            return audioData;
        }
    }

    public async UniTask<VoiceDesignResponse> VoiceDesignAsync(string voiceDescription, string text)
    {
        if (string.IsNullOrEmpty(voiceDescription))
        {
            Debug.LogError("ElevenLabs API key is not set.");
            throw new InvalidOperationException("API key not configured.");
        }
        if (string.IsNullOrEmpty(text))
        {
            Debug.LogError("Voice ID must be provided for voice design.");
            throw new ArgumentException("Voice ID is required.");
        }

        string url = $"{apiBaseUrl}/text-to-voice/create-previews?output_format=pcm_22050";
        var requestPayload = new VoiceDesignRequest { text = text, voice_description = voiceDescription };
        string jsonData = JsonUtility.ToJson(requestPayload);
        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            request.SetRequestHeader("xi-api-key", apiKey);
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            await request.SendWebRequest().ToUniTask();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Voice Design API request failed: {request.error}");
                return null;
            }

            string responseText = request.downloadHandler.text;

            var response = JsonUtility.FromJson<VoiceDesignResponse>(responseText);
            return response;
        }
    }

    public async UniTask<SaveVoiceDesignResponse> SaveVoiceDesignAsync(string voiceName, string voiceDescription, string generatedVoiceId)
    {
        if (string.IsNullOrEmpty(voiceDescription))
        {
            Debug.LogError("Voice description must be provided for voice design.");
            throw new ArgumentException("Voice description is required.");
        }
        if (string.IsNullOrEmpty(voiceName))
        {
            Debug.LogError("Voice name must be provided for voice design.");
            throw new ArgumentException("Voice name is required.");
        }
        if (string.IsNullOrEmpty(generatedVoiceId))
        {
            Debug.LogError("Generated voice ID must be provided for voice design.");
            throw new ArgumentException("Generated voice ID is required.");
        }

        string url = $"{apiBaseUrl}/text-to-voice/create-voice-from-preview";
        var requestPayload = new SaveVoiceDesignRequest
        {
            generated_voice_id = generatedVoiceId,
            voice_name = voiceName,
            voice_description = voiceDescription,
        };
        string jsonData = JsonUtility.ToJson(requestPayload);
        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            request.SetRequestHeader("xi-api-key", apiKey);
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            await request.SendWebRequest().ToUniTask();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Voice Design API request failed: {request.error}");
                return null;
            }

            string responseText = request.downloadHandler.text;

            var response = JsonUtility.FromJson<SaveVoiceDesignResponse>(responseText);
            return response;
        }
    }

    [Serializable]
    private class TTSRequest
    {
        public string text;
        public string model_id;
    }

    [Serializable]
    private class VoiceDesignRequest
    {
        public string voice_description;
        public string text;
    }

    [Serializable]
    private class SaveVoiceDesignRequest
    {
        public string voice_description;
        public string voice_name;
        public string generated_voice_id;
    }

    [Serializable]
    public class VoiceDesignResponse
    {
        [JsonProperty("previews")]
        public List<VoiceDesignPreviews> previews;
    }

    [Serializable]
    public class SaveVoiceDesignResponse
    {
        public string voice_id;
    }

    [Serializable]
    public class VoiceDesignPreviews
    {
        public string audio_base_64;
        public string generated_voice_id;
        public string media_type;
        public string duration_secs;
    }
}