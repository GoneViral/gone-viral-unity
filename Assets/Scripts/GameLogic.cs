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

    // Wird aufgerufen von infizierten Spieler wenn er hustet
    static void OnHustArea(GameObject obj, float fDirection, Vector3 center, float fRadius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, fRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject != obj && hitColliders[i].gameObject.name == "mover")
            {
                Vector3 position = hitColliders[i].gameObject.transform.position;
                float angle = Vector3.Angle(center, position);
                if (Mathf.DeltaAngle(fDirection, angle) >= 337.5f && Mathf.DeltaAngle(fDirection, angle) <= 22.5f)
                {
                    hitColliders[i].gameObject.SendMessage("AddVirus", 1.0f - (Vector3.Distance(position, center) / fRadius));
                }
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
