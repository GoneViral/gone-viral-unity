using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class AirConsoleConnector : MonoBehaviour
{
    public PlayerController[] players = new PlayerController[2];

    private bool firstPlayerConnected = false;
    void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;

        GameObject playerHuman = new GameObject("HumanPlayerController");
        players[0] = playerHuman.AddComponent<PlayerController>();
        players[0].type = PlayerType.Human;
        players[0].controlledObject = GameObject.Find("Player");

        GameObject playerVirus = new GameObject("VirusPlayerController");
        players[1] = playerVirus.AddComponent<PlayerController>();
        players[1].type = PlayerType.Virus;
        players[1].controlledObject = GameObject.Find("Virus");
    }

    void OnMessage(int fromDeviceID, JToken data){
        Debug.Log("message from " + fromDeviceID + ", data: " + data);
        if(data["action"] != null && data["action"].ToString().Equals("right")){
            for(int i = 0; i < 2; i++){
                if(players[i].controllerId == fromDeviceID ){
                    players[i].Move(new Vector3(0, 0, 1));
                    return;
                }
            }
        }else if(data["action"] != null && data["action"].ToString().Equals("left")){
            for(int i = 0; i < 2; i++){
                if(players[i].controllerId == fromDeviceID ){
                    players[i].Move(new Vector3(0, 0, -1));
                    return;
                }
            }
        }else if(data["action"] != null && data["action"].ToString().Equals("up")){
            for(int i = 0; i < 2; i++){
                if(players[i].controllerId == fromDeviceID ){
                    players[i].Move(new Vector3(-1, 0, 0));
                    return;
                }
            }
        }else if(data["action"] != null && data["action"].ToString().Equals("down")){
            for(int i = 0; i < 2; i++){
                if(players[i].controllerId == fromDeviceID ){
                    players[i].Move(new Vector3(1, 0, 0));
                    return;
                }
            }
        }
    }

    void OnConnect(int fromDeviceID){
        Debug.Log("<color=green>connected </color>" + fromDeviceID);

        for(int i = 0; i < 2; i++){
            if(players[i].controllerId == -1 ){
                players[i].controllerId = fromDeviceID;
                if(!firstPlayerConnected){
                    SceneManager.LoadScene("Level", LoadSceneMode.Additive);
                    firstPlayerConnected = true;
                } 

                if(players[i].type == PlayerType.Virus){
                    AirConsole.instance.Message (fromDeviceID, "virus");
                }else if(players[i].type == PlayerType.Human){
                    AirConsole.instance.Message (fromDeviceID, "human");
                }
                return;
            }
        }
    }

    void OnDisconnect(int fromDeviceID){
        Debug.Log("<color=red>disconnected </color>" + fromDeviceID);
        for(int i = 0; i < 2; i++){
            if(players[i].controllerId == fromDeviceID ){
                players[i].controllerId = -1;
                return;
            }
        }
    }


    void OnDestroy()
    {
        if(AirConsole.instance != null){
            AirConsole.instance.onMessage -= OnMessage;
        }
    }
}
