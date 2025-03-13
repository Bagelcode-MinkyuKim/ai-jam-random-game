using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterGenerationScene : MonoBehaviour
{
    [SerializeField] List<CharacterGenerationPromptPopup> promptPopups = new List<CharacterGenerationPromptPopup>();
    [SerializeField] List<string> inputResults = new List<string>();
    [SerializeField] CanvasGroup allDoneCanvasGroup;

    void Start()
    {
        CharacterGenerationFlow().Forget();
    }
    string VoiceDesignRequestPrompt =>
"Voice Design helps creators fill the gaps when the exact voice they are looking for isn’t available in the Voice Library. " +
"If you can’t find a suitable voice for your project, you can create one. Note that Voice Design is highly experimental and " +
"Professional Voice Clones (PVC) are currently the highest quality voices on our platform. If there is a PVC available in our library that fits your needs, we recommend using it instead.\n\n" +

"You can find Voice Design by heading to Voices -> My Voices -> Add a new voice -> Voice Design in the ElevenLabs app or via the API. " +
"When you hit generate, we’ll generate three voice options for you. The only charge for using voice design is the number of credits to generate your preview text, " +
"which you are only charged once even though we are generating three samples for you. You can see the number of characters that will be deducted in the 'Text to preview' text box.\n\n" +

"After generating, you’ll have the option to select and save one of the generations, which will take up one of your voice slots.\n\n" +

"API reference\nSee the API reference for Voice Design\nExample app\nA Next.js example app for Voice Design\nPrompting guide\n\n" +

"Voice design types:\n" +
"Realistic Voice Design: Create an original, realistic voice by specifying age, accent/nationality, gender, tone, pitch, intonation, speed, and emotion.\n" +
"- \"A young Indian female with a soft, high voice. Conversational, slow and calm.\"\n" +
"- \"An old British male with a raspy, deep voice. Professional, relaxed and assertive.\"\n" +
"- \"A middle-aged Australian female with a warm, low voice. Corporate, fast and happy.\"\n\n" +

"Character Voice Design: Generate unique voices for creative characters using simpler prompts.\n" +
"- \"A massive evil ogre, troll\"\n" +
"- \"A sassy little squeaky mouse\"\n" +
"- \"An angry old pirate, shouting\"\n\n" +

"Some other characters we’ve had success with include Goblin, Vampire, Elf, Troll, Werewolf, Ghost, Alien, Giant, Witch, Wizard, Zombie, Demon, Devil, Pirate, Genie, Ogre, Orc, Knight, Samurai, Banshee, Yeti, Druid, Robot, Monkey, Monster, Dracula.\n\n" +

"Voice attributes:\n" +
"Attribute (Importance): Options\n" +
"Age (High): Young, Teenage, Adult, Middle-Aged, Old, etc.\n" +
"Accent/Nationality (High): British, Indian, Polish, American, etc.\n" +
"Gender (High): Male, Female, Gender Neutral\n" +
"Tone (Optional): Gruff, Soft, Warm, Raspy, etc.\n" +
"Pitch (Optional): Deep, Low, High, Squeaky, etc.\n" +
"Intonation (Optional): Conversational, Professional, Corporate, Urban, Posh, etc.\n" +
"Speed (Optional): Fast, Quick, Slow, Relaxed, etc.\n" +
"Emotion/Delivery (Optional): Angry, Calm, Scared, Happy, Assertive, Whispering, Shouting, etc.\n" + "make it more than 100 letters and shorter than 1000 letters definitely";

    public async UniTask CharacterGenerationFlow()
    {
        promptPopups.ForEach(prompt =>
   {
       prompt.gameObject.SetActive(false);
   });
        allDoneCanvasGroup.gameObject.SetActive(false);
        allDoneCanvasGroup.alpha = 0f;
        await UniTask.WaitForSeconds(1f);


        int currentIndex = 0;

        for (; currentIndex < promptPopups.Count; currentIndex++)
        {
            await Appear(promptPopups[currentIndex]);

            string inputResult = await promptPopups[currentIndex].WaitForInput();
            inputResults.Add(inputResult);

            await Disappear(promptPopups[currentIndex]);
        }


        //Generate Image
        Message profilePrompt = await OpenApiClient.Instance.SendRequest(new List<Message>()
             {
            new Message() { role = "user", content = $"앞으로 내가 직업이나 인상착의 등을 너에게 말하면 이미지 제너레이트 용 프롬프트를 짧게 한 문장으로 만들어줘. 직업에 대한 설명은 안적어도 되. 경찰이면 cop 정도만 적어줘. 옷에 대한 설명 및 부연 설명은 필요 없어. 모든 프롬프트는 영어로 뽑아줘야해. 직업에 따라서 어울리는 인종을 부여해 줘" },
                        new Message() { role = "user", content = $"Her style is {inputResults[0]}, Her outfit is {inputResults[1]}, Her pesonality is {inputResults[2]}, voice is {inputResults[3]}" }
             });

        string genevaApiPrompt = $"Generate 3D Animation Disney Character depending on users request, prompt is {profilePrompt.content}";

        Message namePrompt = await OpenApiClient.Instance.SendRequest(new List<Message>()
             {
            new Message() { role = "user", content = $"Her style is {inputResults[0]}, Her outfit is {inputResults[1]}, Her pesonality is {inputResults[2]}, voice is {inputResults[3]} and Generate Her name and give just a name" }
             });
        string name = namePrompt.content;

        Sprite profileImage = await GenevaApiClient.Instance.SendRequest(genevaApiPrompt, name);

        Message voicePrompt = await OpenApiClient.Instance.SendRequest(new List<Message>()
             {
            new Message() { role = "user", content = VoiceDesignRequestPrompt },
            new Message() { role = "user", content = $"Her style is {inputResults[0]}, Her outfit is {inputResults[1]}, Her pesonality is {inputResults[2]}, voice is {inputResults[3]} and Say Hi im {name}" }
             });
        VoiceDesignResponse voiceResponse = await ElevenLabsApiClient.Instance.VoiceDesignAsync($"Dream Chat Voice Design {name}", $"{voicePrompt.content}");
        SaveVoiceDesignResponse voiceSaveResponse = await ElevenLabsApiClient.Instance.SaveVoiceDesignAsync($"Dream Chat Voice Design {name}", $"Dream Chat Voice Design {name}", voiceResponse.previews[0].generated_voice_id);

        CharacterData.CloneWithNewPromptsAndBasicInfo(profileImage, name, voiceSaveResponse.voice_id, inputResults[0], inputResults[1], inputResults[2], inputResults[3]);

        allDoneCanvasGroup.gameObject.SetActive(true);
        allDoneCanvasGroup.alpha = 0f;
        await allDoneCanvasGroup.TweenAlphaAsync(1f, 1f, EaseType.EaseInCubic);

        await UniTask.WaitForSeconds(1f);

        await allDoneCanvasGroup.TweenAlphaAsync(0f, 1f, EaseType.EaseInCubic);

        await UniTask.WaitForSeconds(1f);

        CustomEvent.Trigger(ScreenFlow.Instance.gameObject, "OnChangeMainScreen");
    }

    private async UniTask Appear(CharacterGenerationPromptPopup promptPopup)
    {
        Vector2 from = new Vector2(1200f, 0f);
        Vector2 to = new Vector2(0f, 0f);
        float duration = 0.6f;

        promptPopup.gameObject.SetActive(true);
        promptPopup.rectTransform.anchoredPosition = from;
        await promptPopup.rectTransform.TweenAnchoredPositionAsync(to, duration, EaseType.EaseInCubic);
    }

    private async UniTask Disappear(CharacterGenerationPromptPopup promptPopup)
    {
        Vector2 from = new Vector2(0f, 0f);
        Vector2 to = new Vector2(-1200f, 0f);
        float duration = 0.6f;

        await promptPopup.rectTransform.TweenAnchoredPositionAsync(to, duration, EaseType.EaseInCubic);
        promptPopup.gameObject.SetActive(false);
    }
}
