using UnityEngine;
using System.Collections.Generic;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSourcePrefab;

    [Header("Audio Clips")]
    public AudioClip chatBgmActive;
    public AudioClip chatBgmFlutter;
    public AudioClip chatBgmNormal;
    public AudioClip lobbyBgm;
    public AudioClip messageReceive;
    public AudioClip messageSend;
    public AudioClip type;
    public AudioClip uiSwipe1;
    public AudioClip uiSwipe2;

    [Header("Volume Settings")]
    [Range(0, 1)] public float bgmVolume = 1f;
    [Range(0, 1)] public float sfxVolume = 1f;

    private List<AudioSource> activeSfxSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        // Clean up finished SFX sources
        activeSfxSources.RemoveAll(source => source == null);
    }

    // BGM Controls
    public void PlayBGM(AudioClip clip, float Volume)
    {
        if (bgmSource.clip == clip) return;
        if (bgmSource.isPlaying) bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.volume = Volume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying) bgmSource.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = bgmVolume;
    }

    // SFX Controls
    public void PlaySFX(AudioClip clip, float Volume)
    {
        AudioSource newSfx = Instantiate(sfxSourcePrefab, transform);
        newSfx.clip = clip;
        newSfx.volume = Volume;
        newSfx.Play();
        Destroy(newSfx.gameObject, clip.length);
        activeSfxSources.Add(newSfx);
    }

    public void PlaySFXRandomPitch(AudioClip clip, float Volume)
    {
        AudioSource newSfx = Instantiate(sfxSourcePrefab, transform);
        newSfx.clip = clip;
        newSfx.volume = Volume;
        float random = UnityEngine.Random.Range(0, 300);
        newSfx.pitch = -1;

        if (random < 100) newSfx.pitch = -1;
        else if (random < 200) newSfx.pitch = 0;
        else newSfx.pitch = 1;
        newSfx.Play();
        Destroy(newSfx.gameObject, clip.length);
        activeSfxSources.Add(newSfx);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        foreach (var sfx in activeSfxSources)
        {
            if (sfx != null) sfx.volume = sfxVolume;
        }
    }

    // Example Methods to Play Specific Sounds
    public void PlayChatBGMActive() => PlayBGM(chatBgmActive, 0.6f);
    public void PlayChatBGMFlutter() => PlayBGM(chatBgmFlutter, 0.6f);
    public void PlayChatBGMNormal() => PlayBGM(chatBgmNormal, 0.6f);
    public void PlayLobbyBGM() => PlayBGM(lobbyBgm, 0.6f);

    public void PlayMessageReceive() => PlaySFX(messageReceive, 0.6f);
    public void PlayMessageSend() => PlaySFX(messageSend, 0.6f);
    public void PlayType() => PlaySFXRandomPitch(type, 1f);
    public void PlayUISwipe1() => PlaySFX(uiSwipe1, 0.6f);
    public void PlayUISwipe2() => PlaySFX(uiSwipe2, 0.6f);
}
