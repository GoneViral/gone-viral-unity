using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerType{
    Human,
    Virus
};

public class PlayerController : MonoBehaviour
{

    public GameObject controlledObject;
    public int controllerId = -1;

    public PlayerType type;

    private HumanPlayable[] possibleHumans;
    private int possibleHumansID = 0;

    public void Move(Vector3 direction){
        if(controlledObject != null){
            NavMeshAgent agent = controlledObject.GetComponent<NavMeshAgent>();
            agent.SetDestination(controlledObject.transform.position + direction);
        }      
    }

    public void SwitchControlled(){
        if(type == PlayerType.Human){
            if(possibleHumans == null || possibleHumans.Length == 0){
                possibleHumans = GameObject.FindObjectsOfType<HumanPlayable>();
            }
            possibleHumansID++;
            if(possibleHumansID > possibleHumans.Length -1 ) possibleHumansID = 0;

            controlledObject = possibleHumans[possibleHumansID].gameObject;
        }
    }


}
