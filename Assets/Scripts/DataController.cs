using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class DataController : MonoBehaviour
{
    public static DataController Instance;

    public InputField InputName;
    public string playerName;

    private void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerName()
    { // 플레이어 이름을 저장할 메서드
        playerName = InputName.text;
        SavePlayerInfo();
    }

    // --------------------------------------------------JSON----------------------------------------
    [System.Serializable]
    class PlayerInfo
    {
        public string playerName;
    }

    public void SavePlayerInfo()
    {
        PlayerInfo playerData = new PlayerInfo();
        playerData.playerName = playerName;

        string json = JsonUtility.ToJson(playerData);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadPlayerInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerInfo playerData = JsonUtility.FromJson<PlayerInfo>(json);

            playerName = playerData.playerName;
        }
    }

}
