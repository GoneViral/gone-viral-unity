using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType{
    Human,
    Virus
};

public class PlayerController : MonoBehaviour
{

    public GameObject controlledObject;
    public int controllerId = -1;

    public PlayerType type;

    public void Move(Vector3 direction){
        if(controlledObject != null)
            controlledObject.transform.position += direction;
    }


}
