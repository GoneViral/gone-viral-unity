using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{

    public GameObject NPCPrefab;

    public int NPCCount = 50;
    public int HumanPlayerCount = 5;
    // Start is called before the first frame update

    void Awake(){
        SpawnNPCs();
    }
    public void SpawnNPCs(){
        for(int i = 0; i < NPCCount; i++){
            GameObject GO = Instantiate(NPCPrefab, GetRandomPoint(Vector3.zero, 30f), Quaternion.identity);
            GO.transform.SetParent(gameObject.transform);
            GO.AddComponent<People>();
            //GO.AddComponent<NPCHust>();
            if(i < HumanPlayerCount){
                GO.AddComponent<HumanPlayable>();
            }
            People people = GO.GetComponent<People>();
            GameLogic.instance.NPCsCountTotal ++;
            people.counter = GameLogic.instance.NPCsCountTotal;

            GO.GetComponentInChildren<TextMesh>().text = people.counter.ToString();
        }
    }


     // Get Random Point on a Navmesh surface
    public static Vector3 GetRandomPoint(Vector3 center, float maxDistance) {
        // Get Random Point inside Sphere which position is center, radius is maxDistance
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;

        NavMeshHit hit; // NavMesh Sampling Info Container

        // from randomPos find a nearest point on NavMesh surface in range of maxDistance
        NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

        return hit.position;
    }
}
