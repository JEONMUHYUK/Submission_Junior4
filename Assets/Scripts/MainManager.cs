using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public TextMeshProUGUI playerNameText; 
    private string playerName; // 플레이어 이름을 할당할 변수.

    // Start is called before the first frame update
    void Start()
    {
        

        
        LoadPlayerName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadPlayerName()
    {   // Json으로 저장된 플레이어 이름을 불러온다.
        DataController.Instance.LoadPlayerInfo();
        playerName = DataController.Instance.playerName; 

        playerNameText.text = "Player [ " + playerName + " ]";
    }
}
