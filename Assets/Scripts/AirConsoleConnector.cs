using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

public class AirConsoleConnector : MonoBehaviour
{
    public PlayerController[] players = new PlayerController[2];

    public GameObject startPanel, endPanel, winScreen, loseScreen;

    private bool firstPlayerConnected = false;
    void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;

        GameObject playerHuman = new GameObject("HumanPlayerController");
        players[0] = playerHuman.AddComponent<PlayerController>();
        players[0].type = PlayerType.Human;
        

        GameObject playerVirus = new GameObject("VirusPlayerController");
        players[1] = playerVirus.AddComponent<PlayerController>();
        players[1].type = PlayerType.Virus;
        
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
        }else if (data["action"] != null && data["action"].ToString().Equals("switch")){
            for(int i = 0; i < 2; i++){
                if(players[i].controllerId == fromDeviceID ){
                    int newCounter = players[i].SwitchControlled();
                    var message = new {
                        action = "setNPC",
                        info = new {id = newCounter}
                    };
                    AirConsole.instance.Message (fromDeviceID, message);
                    return;
                }
            }
        } else if(data["action"] != null && data["action"].ToString().Equals("action"))
        {
            for(int i = 0; i < 2; i++){
                if(players[i].controllerId == fromDeviceID ){
                    players[i].Action();
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
                    StartCoroutine(LoadYourAsyncScene());
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


    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("GameScene_Graph", LoadSceneMode.Additive);


        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        players[0].controlledObject = GameObject.Find("PlayerStart");
        players[0].controlledObject.GetComponent<HumanPlayable>().hair.material.color = Color.blue;
        players[1].controlledObject = GameObject.Find("VirusStart");

        InvokeRepeating("CheckGameOver", 0f, 0.5f);
        startPanel.SetActive(false);
    }


    private void CheckGameOver(){
        GameLogic.instance.playtime -= 0.5f;
        if(GameLogic.instance.playtime == 0){
            endPanel.SetActive(true);
            int winnerId = 0;
            if(GameLogic.instance.infectedCount >= GameLogic.instance.NPCsCountTotal){
                //Virus win
                loseScreen.SetActive(true);
                winScreen.SetActive(false);
                winnerId = players[1].controllerId;
            }else if(GameLogic.instance.quarantinedCount <= Mathf.RoundToInt((float) GameLogic.instance.NPCsCountTotal * 0.7f)){
                //Virus win
                loseScreen.SetActive(true);
                winScreen.SetActive(false);
                Debug.Log("virus win not enough quarantined");
                winnerId = players[1].controllerId;
            }else{
                //Human win
                loseScreen.SetActive(false);
                winScreen.SetActive(true);
                winnerId = players[0].controllerId;
            }

            for(int i = 0; i < 2; i++){
                var message = new {
                    action = "gameOver",
                    win = players[i].controllerId == winnerId
                };
                AirConsole.instance.Message (players[i].controllerId, message);
            }
            CancelInvoke();
        }
    }
}
