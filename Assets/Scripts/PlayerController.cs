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

    private VirusPlayable[] possibleVirus;
    private int possibleHumansID = 0;

    public void Move(Vector3 direction){
        if(controlledObject != null){
            NavMeshAgent agent = controlledObject.GetComponent<NavMeshAgent>();
            agent.SetDestination(controlledObject.transform.position + direction);
        }      
    }

    public int SwitchControlled(){
        if(type == PlayerType.Human){
            if(possibleHumans == null || possibleHumans.Length == 0){
                possibleHumans = GameObject.FindObjectsOfType<HumanPlayable>();
            }
            possibleHumansID++;
            if(possibleHumansID > possibleHumans.Length -1 ) possibleHumansID = 0;
            controlledObject = possibleHumans[possibleHumansID].gameObject;
            foreach(HumanPlayable human in possibleHumans){
                human.hair.material.color = Color.cyan;
            }
            controlledObject.GetComponent<HumanPlayable>().hair.material.color = Color.blue;
        }
        else if(type == PlayerType.Virus){
            possibleVirus = GameObject.FindObjectsOfType<VirusPlayable>();

            int index = Random.Range(0, possibleVirus.Length);
            controlledObject = possibleVirus[index].gameObject;
        }
        int counter = controlledObject.GetComponent<People>().counter;
        return counter;
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
            if (hitColliders[i].gameObject != controlledObject && hitColliders[i].gameObject.GetComponent<People>() != null && hitColliders[i].gameObject.GetComponent<People>().status != Status.InHospital)
            {
                People p = hitColliders[i].gameObject.GetComponent<People>();
                p.status = Status.InQuarantine;
                GameLogic.instance.quarantinedCount ++;

                NPCRandomWalk agent = hitColliders[i].gameObject.GetComponent<NPCRandomWalk>();
                if(agent != null){
                    agent.enabled = false;
                }
            }
            i++;
        }
    }

}
