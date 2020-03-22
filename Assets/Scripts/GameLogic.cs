using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance;
    public int NPCsCountTotal = 2;
    public int infectedCount = 1;
    public int quarantinedCount = 0;

    public float playtime = 120f;

    void Awake ()
    {
        if (instance == null)
            instance = this;  
        else if (instance != this)
            Destroy (gameObject);   
    }
}
