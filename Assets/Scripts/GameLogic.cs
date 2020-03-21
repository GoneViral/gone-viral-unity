using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class GameLogic : MonoBehaviour
{
    
    void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        
    }

    void OnMessage(int fromDeviceID, JToken data){
        Debug.Log("message from " + fromDeviceID + ", data: " + data);
        if(data["action"] != null && data["action"].ToString().Equals("action"))
        {
            Camera.main.backgroundColor = Color.green;
        }
        else if (data["action"] != null && data["action"].ToString().Equals("switch"))
        
        {
            Camera.main.backgroundColor = Color.red;
        }
    }

    void OnDestroy()
    {
        if(AirConsole.instance != null){
            AirConsole.instance.onMessage -= OnMessage;
        }
    }
}
