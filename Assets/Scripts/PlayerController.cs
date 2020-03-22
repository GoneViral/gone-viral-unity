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

    public int radius = 4;
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

    public void Action(){
        if(type == PlayerType.Virus){
            return;
        }
        else if(type == PlayerType.Human){
            setQuarantine();
        }
    }

    private void setQuarantine(){
        Vector3 center = controlledObject.transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        Debug.Log("hit" + hitColliders.Length);
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject != controlledObject && hitColliders[i].gameObject.GetComponent<People>() != null)
            {
                Debug.Log("setting in quarantine");
                People p = hitColliders[i].gameObject.GetComponent<People>();
                p.status = Status.InQuarantine;

                NPCRandomWalk agent = hitColliders[i].gameObject.GetComponent<NPCRandomWalk>();
                if(agent != null){
                    agent.enabled = false;
                }
            }
            Debug.Log("done with quarantine");
            i++;
        }
    }

}
