using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayable : MonoBehaviour
{
    public Renderer hair;
    // Start is called before the first frame update
    void Start()
    {
        var hairGO = transform.Find("hair01/default");
        hair = hairGO.GetComponent<Renderer>();

        if(hair != null){
            hair.material.color = Color.blue;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
