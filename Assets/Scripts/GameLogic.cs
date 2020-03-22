﻿using System.Collections;
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

    /*
    //TODO implement showsSymptoms Boolean
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
    }*/

}
