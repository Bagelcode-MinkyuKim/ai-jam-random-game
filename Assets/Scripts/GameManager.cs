using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CharacterData characterData;

    private void Awake()
    {
        Instance = this;
    }
    public void SetupChatting(CharacterData data)
    {
        characterData = data;
    }
}

